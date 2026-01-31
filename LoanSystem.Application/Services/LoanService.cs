using LoanSystem.Application.DTOs;
using LoanSystem.Application.Interfaces;
using LoanSystem.Domain.Entities;
using LoanSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LoanSystem.Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly ApplicationDbContext _context;

        public LoanService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LoanDto> CreateLoanAsync(CreateLoanRequest request)
        {
            var loan = new Loan
            {
                UserId = request.UserId,
                BookId = request.BookId,
                LoanDate = DateTime.UtcNow,
                ReturnDate = null
            };

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            // Fetch the book title for the response
            var book = await _context.Books.FindAsync(request.BookId);

            return new LoanDto
            {
                Id = loan.Id,
                BookTitle = book.Title,
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate
            };
        }

        public async Task<List<LoanDto>> GetUserLoansAsync(int userId)
        {
            var loans = await _context.Loans
                .Where(l => l.UserId == userId)
                .Include(l => l.Book)
                .Select(l => new LoanDto
                {
                    Id = l.Id,
                    BookTitle = l.Book.Title,
                    LoanDate = l.LoanDate,
                    ReturnDate = l.ReturnDate
                })
                .ToListAsync();

            return loans;
        }
    }
}
