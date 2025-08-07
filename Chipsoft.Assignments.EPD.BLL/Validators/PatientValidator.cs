using Chipsoft.Assignments.EPD.Domain;
using FluentValidation;

namespace Chipsoft.Assignments.EPD.BLL.Validators;

public class PatientValidator : PersonValidator<Patient>
{
    public PatientValidator()
    {
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MinimumLength(6).WithMessage("Address must be at least 6 characters")
            .MaximumLength(150).WithMessage("Address cannot be longer than 150 characters");
    }
}