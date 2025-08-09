using Chipsoft.Assignments.EPD.BLL.Dto;
using Chipsoft.Assignments.EPD.BLL.Validators;
using Chipsoft.Assignments.EPD.DAL;
using Chipsoft.Assignments.EPD.Domain;

namespace Chipsoft.Assignments.EPD.BLL.Managers;

public class AppointmentManager(
    IPatientRepository patientRepository,
    IPhysicianRepository physicianRepository,
    IAppointmentRepository appointmentRepository) : IAppointmentManager
{
    public OperationResult AddAppointment(AddAppointmentDto appointmentDto)
    {
        var patient = patientRepository.Read(appointmentDto.PatientId);
        var physician = physicianRepository.Read(appointmentDto.PhysicianId);

        if (patient == null || physician == null)
        {
            return new OperationResult { Success = false, Errors = ["Patient or physician does not exist."] };
        }

        var patientAppointments = appointmentRepository.ReadAppointmentsOfPatient(patient.Id);
        var physicianAppointments = appointmentRepository.ReadAppointmentsOfPhysician(physician.Id);
        var appointments = patientAppointments.Union(physicianAppointments);

        var appointment = new Appointment
        {
            Patient = patient, Physician = physician, StartTime = appointmentDto.DateTime,
            EndTime = appointmentDto.DateTime.AddMinutes(appointmentDto.Duration)
        };

        var validator = new AppointmentValidator(appointments);
        var result = validator.Validate(appointment);
        if (result.IsValid)
        {
            appointmentRepository.Create(appointment);
        }
        
        return result.IsValid
            ? new OperationResult { Success = true }
            : new OperationResult { Success = false, Errors = result.Errors.Select(x => x.ErrorMessage).ToList() };
    }

    public IEnumerable<ShowAppointmentDto> GetAllAppointmentsOfPatient(Guid patientId)
    {
        return appointmentRepository.ReadAppointmentsOfPatient(patientId).Select(ShowAppointmentDto.FromAppointment);
    }

    public IEnumerable<ShowAppointmentDto> GetAllAppointmentsOfPhysician(Guid physicianId)
    {
        return appointmentRepository.ReadAppointmentsOfPhysician(physicianId).Select(ShowAppointmentDto.FromAppointment);
    }
}