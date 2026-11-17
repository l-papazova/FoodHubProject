using FoodHubProject2.Repositories;

namespace FoodHubProject2.Models
{
    public class Role : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

      public ICollection<User> Users { get; set; } = new List<User>();
        IRepository<Role> roleRepository;
    }
}
