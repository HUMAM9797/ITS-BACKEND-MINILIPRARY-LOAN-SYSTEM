using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LoanSystem.Application.DTOs;
using LoanSystem.Application.Interfaces;
using LoanSystem.Domain.Entities;
using LoanSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LoanSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(RegisterUserDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                throw new Exception("Email already exists.");
            }

            UserRole role;
            if (!Enum.TryParse(dto.Role, true, out role))
            {
                // Default to Customer if invalid? Or throw. 
                // Prompt implies role is sent. I'll throw to be safe.
                throw new Exception("Invalid role.");
            }
            
            // Security check: If enforcing role safety, maybe check if role is valid enum?
            if (!Enum.IsDefined(typeof(UserRole), role))
            {
                throw new Exception("Invalid role value.");
            }

            string passwordHash = HashPassword(dto.Password);

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Role = role,
                PasswordHash = passwordHash
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "User registered successfully.";
        }

        public async Task<string> LoginAsync(LoginUserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid credentials.");
            }

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "SuperSecretKeyForLoanSystemDevelopmentOnly123!";
            
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"] ?? "LoanSystem",
                audience: jwtSettings["Audience"] ?? "LoanSystemUsers",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }
    }
}
