using Domain.Models;
using FluentValidation;

namespace Application.Validators
{
    public class EventValidator : AbstractValidator<Event>
    {
        public EventValidator()
        {
            RuleFor(e => e.Name)
                .NotEmpty().WithMessage("Event name is required.")
                .MaximumLength(100).WithMessage("Event name cannot exceed 100 characters.");

            RuleFor(e => e.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(e => e.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(200).WithMessage("Location cannot exceed 200 characters.");

            RuleFor(e => e.EventDate)
                .GreaterThan(DateTime.Now).WithMessage("Event date must be in the future.");
        }
    }
}
