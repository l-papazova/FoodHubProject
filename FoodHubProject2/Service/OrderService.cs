using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodHubProject2.Models;
using FoodHubProject2.Repositories;

namespace FoodHubProject2.Services
{
    public class OrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Restaurant> _restaurantRepository;

        public OrderService(
            IRepository<Order> orderRepository,
            IRepository<Restaurant> restaurantRepository)
        {
            _orderRepository = orderRepository;
            _restaurantRepository = restaurantRepository;
        }

        // Проверка за минимална сума
        public bool IsMinimumOrderAmountValid(
            decimal foodAmount,
            Restaurant restaurant)
        {
            if (restaurant == null || foodAmount < 0)
                return false;

            return foodAmount >= restaurant.MinimumOrderAmount;
        }

        // Изчисляване на крайната сума
        public decimal CalculateFinalAmount(
            decimal foodAmount,
            decimal deliveryFee,
            decimal discount = 0)
        {
            if (foodAmount < 0 || deliveryFee < 0 || discount < 0)
                throw new ArgumentException(
                    "Сумите не могат да бъдат отрицателни.");

            decimal total = foodAmount + deliveryFee - discount;

            if (total < 0)
                throw new ArgumentException(
                    "Отстъпката не може да надвишава сумата.");

            return Math.Round(total, 2);
        }

        // Проверка дали поръчката може да бъде направена
        public async Task<bool> CanPlaceOrderAsync(
            int restaurantId,
            decimal foodAmount)
        {
            var restaurant =
                await _restaurantRepository.GetByIdAsync(restaurantId);

            if (restaurant == null)
                return false;

            if (!restaurant.IsActive)
                return false;

            return IsMinimumOrderAmountValid(foodAmount, restaurant);
        }

        // Изчисляване на общата сума за поръчка
        public async Task<decimal> CalculateOrderTotalAsync(
            int restaurantId,
            decimal foodAmount,
            decimal discount = 0)
        {
            var restaurant =
                await _restaurantRepository.GetByIdAsync(restaurantId);

            if (restaurant == null)
                throw new ArgumentException("Заведението не съществува.");

            if (!IsMinimumOrderAmountValid(foodAmount, restaurant))
            {
                throw new InvalidOperationException(
                    "Минималната сума за поръчка не е достигната.");
            }

            return CalculateFinalAmount(
                foodAmount,
                restaurant.DeliveryFee,
                discount);
        }

        // Взимане на всички поръчки
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        // Намиране на поръчка по ID
        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        // Проследяване на поръчка
        public async Task<string> GetOrderStatusAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
                return "Поръчката не е намерена.";

            return order.Status;
        }

        // Промяна на статуса на поръчката
        public async Task UpdateOrderStatusAsync(
            int orderId,
            string newStatus)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
                throw new ArgumentException("Поръчката не съществува.");

            var allowedStatuses = new[]
            {
                "Приета",
                "В подготовка",
                "В доставка",
                "Доставена",
                "Отказана"
            };

            if (!allowedStatuses.Contains(newStatus))
            {
                throw new ArgumentException(
                    "Невалиден статус на поръчката.");
            }

            if (!IsValidStatusTransition(order.Status, newStatus))
            {
                throw new InvalidOperationException(
                    "Невалиден преход между статусите.");
            }

            order.Status = newStatus;

            await _orderRepository.UpdateAsync(order);
        }

        // Проверка на преходите между статусите
        private bool IsValidStatusTransition(
            string currentStatus,
            string newStatus)
        {
            if (currentStatus == newStatus)
                return true;

            return currentStatus switch
            {
                "Приета" =>
                    newStatus == "В подготовка" ||
                    newStatus == "Отказана",

                "В подготовка" =>
                    newStatus == "В доставка" ||
                    newStatus == "Отказана",

                "В доставка" =>
                    newStatus == "Доставена",

                "Доставена" => false,

                "Отказана" => false,

                _ => false
            };
        }
    }
}