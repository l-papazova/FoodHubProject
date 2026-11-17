using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodHubProject2.Models;

namespace FoodHubProject2.Repositories
{
    public class InMemoryRepository<T> : IRepository<T>
        where T : class, IEntity
    {
        private readonly List<T> _items = new List<T>();

        private readonly object _lock = new object();

        // Взимане на всички записи
        public Task<IEnumerable<T>> GetAllAsync()
        {
            lock (_lock)
            {
                IEnumerable<T> result = _items.ToList();

                return Task.FromResult(result);
            }
        }

        // Взимане на запис по ID
        public Task<T?> GetByIdAsync(int id)
        {
            lock (_lock)
            {
                var item = _items.FirstOrDefault(x => x.Id == id);

                return Task.FromResult(item);
            }
        }

        // Добавяне на нов запис
        public Task AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            lock (_lock)
            {
                if (entity.Id == 0)
                {
                    int nextId = _items.Count > 0
                        ? _items.Max(x => x.Id) + 1
                        : 1;

                    entity.Id = nextId;
                }

                if (_items.Any(x => x.Id == entity.Id))
                {
                    throw new InvalidOperationException(
                        "Запис с това ID вече съществува.");
                }

                _items.Add(entity);
            }

            return Task.CompletedTask;
        }

        // Обновяване на запис
        public Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            lock (_lock)
            {
                var existing = _items.FirstOrDefault(
                    x => x.Id == entity.Id);

                if (existing != null)
                {
                    int index = _items.IndexOf(existing);

                    _items[index] = entity;
                }
            }

            return Task.CompletedTask;
        }

        // Изтриване на запис
        public Task DeleteAsync(int id)
        {
            lock (_lock)
            {
                var existing = _items.FirstOrDefault(
                    x => x.Id == id);

                if (existing != null)
                {
                    _items.Remove(existing);
                }
            }

            return Task.CompletedTask;
        }
    }
}
