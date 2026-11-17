using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FoodHubProject2.Services;

namespace FoodHubProject2.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly RestaurantService _restaurantService;

        public RestaurantController(
            RestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        // Каталог със заведения
        public async Task<IActionResult> Index()
        {
            var restaurants =
                await _restaurantService.GetAllRestaurantsAsync();

            return View(restaurants);
        }

        // Детайли за конкретно заведение
        public async Task<IActionResult> Details(int id)
        {
            var restaurant =
                await _restaurantService.GetRestaurantByIdAsync(id);

            if (restaurant == null)
                return NotFound();

            return View(restaurant);
        }

        // Само отворени заведения
        public async Task<IActionResult> OpenRestaurants()
        {
            var restaurants =
                await _restaurantService.GetOpenRestaurantsAsync();

            return View("Index", restaurants);
        }
    }
}