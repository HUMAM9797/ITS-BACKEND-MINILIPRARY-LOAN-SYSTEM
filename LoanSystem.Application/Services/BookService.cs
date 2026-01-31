using LoanSystem.Application.DTOs;
using LoanSystem.Application.Interfaces;
using LoanSystem.Domain.Entities;
using LoanSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LoanSystem.Application.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateBookAsync(CreateBookRequest request)
        {
            var book = new Book
            {
                Title = request.Title
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book.Id;
        }

        public async Task<bool> BookExistsAsync(int bookId)
        {
            return await _context.Books.AnyAsync(b => b.Id == bookId);
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            return await _context.Books
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title
                })
                .ToListAsync();
        }

        public async Task<PagedResult<BookDto>> GetBooksPagedAsync(int page, int pageSize)
        {
            var query = _context.Books.AsQueryable();
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookDto { Id = b.Id, Title = b.Title })
                .ToListAsync();

            return new PagedResult<BookDto>
            {
                Items = items,
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
    }
}
