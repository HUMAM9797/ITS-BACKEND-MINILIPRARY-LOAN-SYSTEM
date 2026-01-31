using LoanSystem.Domain.Interfaces;

namespace LoanSystem.Domain.Entities
{
    public class Loan : IAuditable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public string? Changes { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public Book Book { get; set; }
    }
}
