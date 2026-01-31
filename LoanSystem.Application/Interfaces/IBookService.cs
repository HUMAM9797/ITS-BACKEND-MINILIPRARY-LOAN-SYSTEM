using LoanSystem.Application.DTOs;

namespace LoanSystem.Application.Interfaces
{
    public interface IBookService
    {
        Task<int> CreateBookAsync(CreateBookRequest request);
        Task<bool> BookExistsAsync(int bookId);
        Task<IEnumerable<BookDto>> GetAllBooksAsync();
        Task<PagedResult<BookDto>> GetBooksPagedAsync(int page, int pageSize);
    }
}
