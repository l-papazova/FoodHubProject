using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FoodHubProject2.Services;

namespace FoodHubProject2.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // Всички поръчки
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            return View(orders);
        }

        // Проследяване на поръчка
        public async Task<IActionResult> Track(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}