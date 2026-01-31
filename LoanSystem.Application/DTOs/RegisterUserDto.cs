using System.ComponentModel.DataAnnotations;

namespace LoanSystem.Application.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // "Admin" or "Customer", or "1", "2"
    }
}
