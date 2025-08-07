using Chipsoft.Assignments.EPD.Domain;
using FluentValidation;

namespace Chipsoft.Assignments.EPD.BLL.Validators;

public abstract class PersonValidator<T> : AbstractValidator<T> where T : Person
{
    protected PersonValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters")
            .MaximumLength(50).WithMessage("First name cannot be longer than 50 characters");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters")
            .MaximumLength(50).WithMessage("Last name cannot be longer than 50 characters");
    }
}