using Chipsoft.Assignments.EPD.Domain;
using FluentValidation;

namespace Chipsoft.Assignments.EPD.BLL.Validators;

public class PhysicianValidator : PersonValidator<Physician>
{
    public PhysicianValidator()
    {
        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("Department is required")
            .MinimumLength(3).WithMessage("Department must be at least 3 characters")
            .MaximumLength(50).WithMessage("Department cannot be longer than 50 characters");
    }
}