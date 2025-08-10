using Chipsoft.Assignments.EPD.BLL.Dto;

namespace Chipsoft.Assignments.EPD.BLL.Managers;

public interface IAppointmentManager
{
    OperationResult AddAppointment(AddAppointmentDto appointmentDto);
    IEnumerable<ShowAppointmentDto> GetAllAppointments();
    IEnumerable<ShowAppointmentDto> GetAllAppointmentsOfPatient(Guid patientId);
    IEnumerable<ShowAppointmentDto> GetAllAppointmentsOfPhysician(Guid physicianId);
}