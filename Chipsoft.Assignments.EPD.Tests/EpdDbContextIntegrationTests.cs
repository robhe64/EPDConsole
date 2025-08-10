using Chipsoft.Assignments.EPD.DAL.EF;
using Chipsoft.Assignments.EPD.Domain;
using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPD.Tests;

public class EpdDbContextIntegrationTests
{
    private readonly EpdDbContext _context;

    public EpdDbContextIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<EpdDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new EpdDbContext(options);
        _context.Database.EnsureCreated();
    }

    [Fact]
    public void DeletingPatient_CascadeDeletesAppointments()
    {
        // Arrange
        var patient = new Patient("John", "Doe", "Address");
        var physician = new Physician("Jane", "Smith", "Cardiology");
        var appointment = new Appointment(DateTime.Now, DateTime.Now.AddMinutes(30), patient, physician);
        _context.Patients.Add(patient);
        _context.Physicians.Add(physician);
        _context.Appointments.Add(appointment);
        _context.SaveChanges();

        // Act
        _context.Patients.Remove(patient);
        _context.SaveChanges();

        // Assert
        var appointmentCount = _context.Appointments.Count();
        Assert.Equal(0, appointmentCount);
    }
    
    [Fact]
    public void DeletingPhysician_CascadeDeletesAppointments()
    {
        // Arrange
        var patient = new Patient("John", "Doe", "Address");
        var physician = new Physician("Jane", "Smith", "Cardiology");
        var appointment = new Appointment(DateTime.Now, DateTime.Now.AddMinutes(30), patient, physician);
        _context.Patients.Add(patient);
        _context.Physicians.Add(physician);
        _context.Appointments.Add(appointment);
        _context.SaveChanges();

        // Act
        _context.Physicians.Remove(physician);
        _context.SaveChanges();

        // Assert
        var appointmentCount = _context.Appointments.Count();
        Assert.Equal(0, appointmentCount);
    }
    
    [Fact]
    public void DeletingAppointment_DoesNotCascadeDeletePatientsOrPhysicians()
    {
        // Arrange
        var patient = new Patient("John", "Doe", "Address");
        var physician = new Physician("Jane", "Smith", "Cardiology");
        var appointment = new Appointment(DateTime.Now, DateTime.Now.AddMinutes(30), patient, physician);
        _context.Patients.Add(patient);
        _context.Physicians.Add(physician);
        _context.Appointments.Add(appointment);
        _context.SaveChanges();

        // Act
        _context.Appointments.Remove(appointment);
        _context.SaveChanges();

        // Assert
        var patientCount = _context.Patients.Count();
        var physicianCount = _context.Physicians.Count();
        Assert.Equal(1, patientCount);
        Assert.Equal(1, physicianCount);
    }

}