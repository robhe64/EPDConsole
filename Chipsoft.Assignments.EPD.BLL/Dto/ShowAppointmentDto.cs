using System.Text;
using Chipsoft.Assignments.EPD.Domain;

namespace Chipsoft.Assignments.EPD.BLL.Dto;

public record ShowAppointmentDto(
    string PhysicianName,
    string PatientName,
    DateTime StartTime,
    DateTime EndTime,
    string Department)
{
    internal static ShowAppointmentDto FromAppointment(Appointment appointment)
    {
        return new ShowAppointmentDto(appointment.Physician.FirstName + " " + appointment.Physician.LastName,
            appointment.Patient.FirstName + " " + appointment.Patient.LastName,
            appointment.StartTime, appointment.EndTime, appointment.Physician.Department);
    }

    public override string ToString()
    {
        return $"Dr. {PhysicianName} has an appointment with {PatientName} at {StartTime.ToShortDateString()} from " +
               $"{StartTime.ToShortTimeString()} to {EndTime.ToShortTimeString()} at the {Department} department.";
    }
}