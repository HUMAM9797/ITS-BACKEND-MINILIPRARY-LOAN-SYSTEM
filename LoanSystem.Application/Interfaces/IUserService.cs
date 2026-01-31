using LoanSystem.Application.DTOs;

namespace LoanSystem.Application.Interfaces
{
    public interface IUserService
    {
        Task<int> CreateUserAsync(CreateUserRequest request);
        Task<bool> UserExistsAsync(int userId);
    }
}
