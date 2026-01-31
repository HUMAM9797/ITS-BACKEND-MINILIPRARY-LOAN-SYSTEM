using LoanSystem.Application.DTOs;
using LoanSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoanSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILoanService _loanService;

        public UsersController(IUserService userService, ILoanService loanService)
        {
            _userService = userService;
            _loanService = loanService;
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateUser([FromBody] CreateUserRequest request)
        {
            var userId = await _userService.CreateUserAsync(request);
            return CreatedAtAction(nameof(CreateUser), new { id = userId }, userId);
        }

        [HttpGet("{userId}/loans")]
        public async Task<ActionResult<List<LoanDto>>> GetUserLoans(int userId)
        {
            var userExists = await _userService.UserExistsAsync(userId);
            if (!userExists)
            {
                return NotFound("User not found");
            }

            var loans = await _loanService.GetUserLoansAsync(userId);
            return Ok(loans);
        }
    }
}
