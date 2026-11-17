using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodHubProject2.Models;
using FoodHubProject2.Repositories;

namespace FoodHubProject2.Services
{
    public class UserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        // Връщане на всички потребители
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        // Намиране на потребител по ID
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        // Проверка дали имейлът вече съществува
        public async Task<bool> EmailExistsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Имейлът е задължителен.");

            var users = await _userRepository.GetAllAsync();

            return users.Any(u =>
                u.Email.Equals(
                    email.Trim(),
                    StringComparison.OrdinalIgnoreCase));
        }

        // Добавяне на нов потребител
        public async Task RegisterUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("Имейлът е задължителен.");

            if (await EmailExistsAsync(user.Email))
            {
                throw new InvalidOperationException(
                    "Потребител с този имейл вече съществува.");
            }

            await _userRepository.AddAsync(user);
        }
    }
}