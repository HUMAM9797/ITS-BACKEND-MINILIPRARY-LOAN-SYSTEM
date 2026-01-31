using LoanSystem.Application.DTOs;
using LoanSystem.Application.Interfaces;
using LoanSystem.Domain.Entities;
using LoanSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LoanSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateUserAsync(CreateUserRequest request)
        {
            var user = new User
            {
                Name = request.Name
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task<bool> UserExistsAsync(int userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }
    }
}
