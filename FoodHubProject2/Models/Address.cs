using FoodHubProject2.Repositories;

namespace FoodHubProject2.Models
{
    public class Address : IEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string City { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;

        public string? StreetNum { get; set; }

        public string? ApartmentNum { get; set; }

        public string PostalCode { get; set; } = string.Empty;

        public bool IsDefault { get; set; }

        public User? User { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        IRepository<Address> addressRepository;
    }
}