using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodHubProject2.Models;
using FoodHubProject2.Repositories;

namespace FoodHubProject2.Services
{
    public class RestaurantService
    {
        private readonly IRepository<Restaurant> _restaurantRepository;

        public RestaurantService(
            IRepository<Restaurant> restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        // Връща всички заведения
        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            return await _restaurantRepository.GetAllAsync();
        }

        // Намира заведение по ID
        public async Task<Restaurant?> GetRestaurantByIdAsync(int id)
        {
            return await _restaurantRepository.GetByIdAsync(id);
        }

        // Проверява дали заведението е отворено
        public bool IsRestaurantOpen(Restaurant restaurant)
        {
            if (restaurant == null || !restaurant.IsActive)
                return false;

            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            // Заведение, което работи през полунощ
            if (restaurant.OpeningTime <= restaurant.ClosingTime)
            {
                return currentTime >= restaurant.OpeningTime &&
                       currentTime <= restaurant.ClosingTime;
            }

            return currentTime >= restaurant.OpeningTime ||
                   currentTime <= restaurant.ClosingTime;
        }

        // Връща всички отворени заведения
        public async Task<IEnumerable<Restaurant>> GetOpenRestaurantsAsync()
        {
            var restaurants = await _restaurantRepository.GetAllAsync();

            return restaurants
                .Where(r => IsRestaurantOpen(r))
                .ToList();
        }

        // Проверява дали заведението е активно
        public bool IsRestaurantActive(Restaurant restaurant)
        {
            return restaurant != null && restaurant.IsActive;
        }
    }
}