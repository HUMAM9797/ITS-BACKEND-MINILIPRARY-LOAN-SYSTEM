using LoanSystem.Application.DTOs;

namespace LoanSystem.Application.Interfaces
{
    public interface ILoanService
    {
        Task<LoanDto> CreateLoanAsync(CreateLoanRequest request);
        Task<List<LoanDto>> GetUserLoansAsync(int userId);
    }
}
