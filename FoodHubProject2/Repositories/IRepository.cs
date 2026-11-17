using System.Collections.Generic;
using System.Threading.Tasks;
using FoodHubProject2.Models;

namespace FoodHubProject2.Repositories
{
    public interface IRepository<T>
        where T : class, IEntity
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);
    }
}
