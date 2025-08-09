using Chipsoft.Assignments.EPD.Domain;
using FluentValidation;

namespace Chipsoft.Assignments.EPD.BLL.Validators;

public class AppointmentValidator : AbstractValidator<Appointment>
{
    public AppointmentValidator(IEnumerable<Appointment> relevantAppointments)
    {
        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required")
            .LessThanOrEqualTo(x => x.EndTime).WithMessage("Start time must be before end time");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End time is required")
            .GreaterThanOrEqualTo(x => x.StartTime).WithMessage("End time must be after start time");

        RuleFor(x => x)
            .Must(x => !IsOverlapping(x, relevantAppointments))
            .WithMessage("Appointment overlaps with another appointment")
            .Must(x => x.EndTime > x.StartTime.AddMinutes(10))
            .WithMessage("Appointment must be at least 10 minutes long")
            .Must(x => x.EndTime < x.StartTime.AddHours(4)).WithMessage("Appointment cannot be longer than 4 hours");
    }

    private bool IsOverlapping(Appointment newAppointment, IEnumerable<Appointment> existingAppointments)
    {
        return existingAppointments.Any(existingAppointment => newAppointment.StartTime < existingAppointment.EndTime &&
                                                               newAppointment.EndTime > existingAppointment.StartTime);
    }
}