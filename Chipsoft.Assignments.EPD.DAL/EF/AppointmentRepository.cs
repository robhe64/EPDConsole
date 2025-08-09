using Chipsoft.Assignments.EPD.Domain;
using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPD.DAL.EF;

public class AppointmentRepository(EpdDbContext context) : Repository<Appointment>(context), IAppointmentRepository
{
    public IEnumerable<Appointment> ReadAppointmentsOfPatient(Guid id)
    {
        return Context.Appointments.Where(a => a.Patient.Id == id).Include(a => a.Physician).ToList();
    }
    
    public IEnumerable<Appointment> ReadAppointmentsOfPhysician(Guid id)
    {
        return Context.Appointments.Where(a => a.Physician.Id == id).Include(a => a.Patient).ToList();
    }
}