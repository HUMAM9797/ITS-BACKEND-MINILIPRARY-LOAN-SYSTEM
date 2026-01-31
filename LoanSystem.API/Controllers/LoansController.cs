using FluentValidation;
using LoanSystem.Application.DTOs;
using LoanSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoanSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly IValidator<CreateLoanRequest> _validator;

        public LoansController(ILoanService loanService, IValidator<CreateLoanRequest> validator)
        {
            _loanService = loanService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<ActionResult<LoanDto>> CreateLoan([FromBody] CreateLoanRequest request)
        {
            // Override UserId from Token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("User ID claim missing.");
            
            request.UserId = int.Parse(userIdClaim.Value);

             // Re-validate needed? 
             // UserId in request body might be ignored or checked. 
             // Validator might check UserId > 0. If I set it here, it's fine.
            
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var loanDto = await _loanService.CreateLoanAsync(request);
            return CreatedAtAction(nameof(CreateLoan), loanDto);
        }
    }
}
