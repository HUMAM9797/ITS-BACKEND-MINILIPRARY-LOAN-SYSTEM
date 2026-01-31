using FluentValidation;
using LoanSystem.Application.DTOs;

namespace LoanSystem.Application.Validators
{
    public class CreateLoanRequestValidator : AbstractValidator<CreateLoanRequest>
    {
        public CreateLoanRequestValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("UserId must be greater than 0.");

            RuleFor(x => x.BookId)
                .GreaterThan(0)
                .WithMessage("BookId must be greater than 0.");
        }
    }
}
