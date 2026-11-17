using FoodHubProject2.Repositories;

namespace FoodHubProject2.Models
{
    public class OrderItem : IEntity
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int MenuItemId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Subtotal { get; set; }

        public Order? Order { get; set; }

        public MenuItem? MenuItem { get; set; }
        IRepository<OrderItem> orderitemRepository;
    }
}
