using LoanSystem.Application.DTOs;

namespace LoanSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterUserDto registerUserDto);
        Task<string> LoginAsync(LoginUserDto loginUserDto);
    }
}
