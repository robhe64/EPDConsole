using Chipsoft.Assignments.EPD.Domain;

namespace Chipsoft.Assignments.EPD.DAL;

public interface IAppointmentRepository : IRepository<Appointment>
{
    IEnumerable<Appointment> ReadAppointmentsOfPatient(Guid id);
    IEnumerable<Appointment> ReadAppointmentsOfPhysician(Guid id);

}