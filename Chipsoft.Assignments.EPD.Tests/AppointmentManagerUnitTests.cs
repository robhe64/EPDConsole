using Chipsoft.Assignments.EPD.BLL.Dto;
using Chipsoft.Assignments.EPD.BLL.Managers;
using Chipsoft.Assignments.EPD.DAL;
using Chipsoft.Assignments.EPD.Domain;
using Moq;

namespace Chipsoft.Assignments.EPD.Tests;

public class AppointmentManagerUnitTests
{
    private readonly Mock<IPatientRepository> _patientRepositoryMock;
    private readonly Mock<IPhysicianRepository> _physicianRepositoryMock;
    private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
    private readonly IAppointmentManager _appointmentManager;

    public AppointmentManagerUnitTests()
    {
        _patientRepositoryMock = new Mock<IPatientRepository>();
        _physicianRepositoryMock = new Mock<IPhysicianRepository>();
        _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        _appointmentManager = new AppointmentManager(
            _patientRepositoryMock.Object,
            _physicianRepositoryMock.Object,
            _appointmentRepositoryMock.Object);
    }

    [Fact]
    public void AddAppointment_WithValidData_ReturnsSuccess_AndCreatesAppointment()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var physicianId = Guid.NewGuid();
        var dateTime = DateTime.Now.AddDays(1);
        var duration = 30;

        var dto = new AddAppointmentDto(patientId, physicianId, dateTime, duration);
        var patient = new Patient("John", "Doe", "2000 Antwerpen") { Id = patientId };
        var physician = new Physician("Jane", "Smith", "Cardiology") { Id = physicianId };

        _patientRepositoryMock.Setup(r => r.Read(patientId)).Returns(patient);
        _physicianRepositoryMock.Setup(r => r.Read(physicianId)).Returns(physician);
        _appointmentRepositoryMock.Setup(r => r.ReadAppointmentsOfPatient(patientId)).Returns(new List<Appointment>());
        _appointmentRepositoryMock.Setup(r => r.ReadAppointmentsOfPhysician(physicianId)).Returns(new List<Appointment>());

        Appointment? createdAppointment = null;
        _appointmentRepositoryMock
            .Setup(r => r.Create(It.IsAny<Appointment>()))
            .Callback<Appointment>(a => createdAppointment = a);

        // Act
        var result = _appointmentManager.AddAppointment(dto);

        // Assert
        Assert.True(result.Success);
        Assert.Empty(result.Errors);
        _appointmentRepositoryMock.Verify(r => r.Create(It.IsAny<Appointment>()), Times.Once);
        Assert.NotNull(createdAppointment);
        Assert.Equal(patient, createdAppointment.Patient);
        Assert.Equal(physician, createdAppointment.Physician);
        Assert.Equal(dateTime, createdAppointment.StartTime);
        Assert.Equal(dateTime.AddMinutes(duration), createdAppointment.EndTime);
    }

    [Fact]
    public void AddAppointment_WithNonExistentPatient_ReturnsError()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var physicianId = Guid.NewGuid();
        var dateTime = DateTime.Now.AddDays(1);
        var duration = 30;

        var dto = new AddAppointmentDto(patientId, physicianId, dateTime, duration);
        var physician = new Physician("Jane", "Smith", "Cardiology") { Id = physicianId };

        _patientRepositoryMock.Setup(r => r.Read(patientId)).Returns((Patient?)null);
        _physicianRepositoryMock.Setup(r => r.Read(physicianId)).Returns(physician);

        // Act
        var result = _appointmentManager.AddAppointment(dto);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Patient or physician does not exist.", result.Errors);
        _appointmentRepositoryMock.Verify(r => r.Create(It.IsAny<Appointment>()), Times.Never);
        _appointmentRepositoryMock.Verify(r => r.ReadAppointmentsOfPatient(It.IsAny<Guid>()), Times.Never);
        _appointmentRepositoryMock.Verify(r => r.ReadAppointmentsOfPhysician(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void AddAppointment_WithNonExistentPhysician_ReturnsError()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var physicianId = Guid.NewGuid();
        var dateTime = DateTime.Now.AddDays(1);
        var duration = 30;

        var dto = new AddAppointmentDto(patientId, physicianId, dateTime, duration);
        var patient = new Patient("John", "Doe", "2000 Antwerpen") { Id = patientId };

        _patientRepositoryMock.Setup(r => r.Read(patientId)).Returns(patient);
        _physicianRepositoryMock.Setup(r => r.Read(physicianId)).Returns((Physician?)null);

        // Act
        var result = _appointmentManager.AddAppointment(dto);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Patient or physician does not exist.", result.Errors);
        _appointmentRepositoryMock.Verify(r => r.Create(It.IsAny<Appointment>()), Times.Never);
        _appointmentRepositoryMock.Verify(r => r.ReadAppointmentsOfPatient(It.IsAny<Guid>()), Times.Never);
        _appointmentRepositoryMock.Verify(r => r.ReadAppointmentsOfPhysician(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void AddAppointment_WithBothNonExistentPatientAndPhysician_ReturnsError()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var physicianId = Guid.NewGuid();
        var dateTime = DateTime.Now.AddDays(1);
        var duration = 30;

        var dto = new AddAppointmentDto(patientId, physicianId, dateTime, duration);

        _patientRepositoryMock.Setup(r => r.Read(patientId)).Returns((Patient?)null);
        _physicianRepositoryMock.Setup(r => r.Read(physicianId)).Returns((Physician?)null);

        // Act
        var result = _appointmentManager.AddAppointment(dto);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Patient or physician does not exist.", result.Errors);
        _appointmentRepositoryMock.Verify(r => r.Create(It.IsAny<Appointment>()), Times.Never);
    }

    [Fact]
    public void AddAppointment_WithValidationErrors_ReturnsError_AndDoesNotCreateAppointment()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var physicianId = Guid.NewGuid();
        var dateTime = DateTime.Now.AddDays(1);
        var duration = 30;

        var dto = new AddAppointmentDto(patientId, physicianId, dateTime, duration);
        var patient = new Patient("John", "Doe", "2000 Antwerpen") { Id = patientId };
        var physician = new Physician("Jane", "Smith", "Cardiology") { Id = physicianId };

        // Create a conflicting appointment to trigger validation failure
        var conflictingAppointment = new Appointment
        {
            Patient = patient,
            Physician = physician,
            StartTime = dateTime,
            EndTime = dateTime.AddMinutes(30)
        };

        _patientRepositoryMock.Setup(r => r.Read(patientId)).Returns(patient);
        _physicianRepositoryMock.Setup(r => r.Read(physicianId)).Returns(physician);
        _appointmentRepositoryMock.Setup(r => r.ReadAppointmentsOfPatient(patientId))
            .Returns(new List<Appointment> { conflictingAppointment });
        _appointmentRepositoryMock.Setup(r => r.ReadAppointmentsOfPhysician(physicianId))
            .Returns(new List<Appointment>());

        // Act
        var result = _appointmentManager.AddAppointment(dto);

        // Assert
        Assert.False(result.Success);
        Assert.NotEmpty(result.Errors);
        Assert.Contains("Appointment overlaps with another appointment", result.Errors);
        _appointmentRepositoryMock.Verify(r => r.Create(It.IsAny<Appointment>()), Times.Never);
    }

    [Fact]
    public void AddAppointment_CreatesAppointmentWithCorrectProperties()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var physicianId = Guid.NewGuid();
        var dateTime = new DateTime(2024, 12, 15, 14, 30, 0);
        var duration = 45;

        var dto = new AddAppointmentDto(patientId, physicianId, dateTime, duration);
        var patient = new Patient("John", "Doe", "2000 Antwerpen") { Id = patientId };
        var physician = new Physician("Jane", "Smith", "Cardiology") { Id = physicianId };

        _patientRepositoryMock.Setup(r => r.Read(patientId)).Returns(patient);
        _physicianRepositoryMock.Setup(r => r.Read(physicianId)).Returns(physician);
        _appointmentRepositoryMock.Setup(r => r.ReadAppointmentsOfPatient(patientId)).Returns(new List<Appointment>());
        _appointmentRepositoryMock.Setup(r => r.ReadAppointmentsOfPhysician(physicianId)).Returns(new List<Appointment>());

        Appointment? createdAppointment = null;
        _appointmentRepositoryMock
            .Setup(r => r.Create(It.IsAny<Appointment>()))
            .Callback<Appointment>(a => createdAppointment = a);

        // Act
        var result = _appointmentManager.AddAppointment(dto);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(createdAppointment);
        Assert.Equal(patient, createdAppointment.Patient);
        Assert.Equal(physician, createdAppointment.Physician);
        Assert.Equal(dateTime, createdAppointment.StartTime);
        Assert.Equal(dateTime.AddMinutes(45), createdAppointment.EndTime);
    }
}
