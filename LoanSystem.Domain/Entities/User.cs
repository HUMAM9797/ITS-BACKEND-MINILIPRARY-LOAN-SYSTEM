using LoanSystem.Domain.Interfaces;

namespace LoanSystem.Domain.Entities
{
    public class User : ISoftDeletable, IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public string? Changes { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation Property
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}
