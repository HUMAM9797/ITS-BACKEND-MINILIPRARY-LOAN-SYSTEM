namespace LoanSystem.Application.DTOs
{
    public class LoanDto
    {
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
