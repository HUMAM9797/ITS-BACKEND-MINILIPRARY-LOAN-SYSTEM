namespace LoanSystem.Domain.Interfaces
{
    public interface IAuditable
    {
        string? Changes { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
