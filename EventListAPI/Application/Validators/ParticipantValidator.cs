using Domain.Models;
using FluentValidation;

namespace Application.Validators
{
    public class ParticipantValidator : AbstractValidator<Participant>
    {
        public ParticipantValidator()
        {
            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MaximumLength(50).WithMessage("Password cannot exceed 50 characters.");
        }
    }
}
