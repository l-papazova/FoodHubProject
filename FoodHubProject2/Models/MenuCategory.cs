using FoodHubProject2.Repositories;

namespace FoodHubProject2.Models
{
    public class MenuCategory : IEntity
    {
        public int Id { get; set; }

        public int RestaurantId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int DisplayOrder { get; set; }

        public Restaurant? Restaurant { get; set; }

        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        IRepository<MenuCategory> menucategoryRepository;
    }
}
