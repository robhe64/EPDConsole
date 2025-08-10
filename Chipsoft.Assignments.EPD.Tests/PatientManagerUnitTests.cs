using Chipsoft.Assignments.EPD.BLL.Dto;
using Chipsoft.Assignments.EPD.BLL.Managers;
using Chipsoft.Assignments.EPD.DAL;
using Chipsoft.Assignments.EPD.Domain;
using Moq;

namespace Chipsoft.Assignments.EPD.Tests;

public class PatientManagerUnitTests
{
    private readonly Mock<IPatientRepository> _patientRepositoryMock;
    private readonly IPatientManager _patientManager;

    public PatientManagerUnitTests()
    {
        _patientRepositoryMock = new Mock<IPatientRepository>();
        _patientManager = new PatientManager(_patientRepositoryMock.Object);
    }

    [Fact]
    public void AddingPatient_WithValidData_ReturnsSuccess_AndAddsPatient()
    {
        // Arrange
        var dto = new AddPatientDto("John", "Doe", "2000 Antwerpen");
        Patient? createdPatient = null;

        _patientRepositoryMock
            .Setup(r => r.Create(It.IsAny<Patient>()))
            .Callback<Patient>(p => createdPatient = p);

        // Act
        var result = _patientManager.AddPatient(dto);

        // Assert
        Assert.True(result.Success);
        _patientRepositoryMock.Verify(r => r.Create(It.IsAny<Patient>()), Times.Once);
        Assert.NotNull(createdPatient);
        Assert.Equal(dto.FirstName, createdPatient.FirstName);
        Assert.Equal(dto.LastName, createdPatient.LastName);
    }

    [Fact]
    public void AddingPatient_WithInvalidData_ReturnsError()
    {
        // Arrange
        var dto = new AddPatientDto("", "Doe", "2000 Antwerpen"); // Invalid: First name is empty

        // Act
        var result = _patientManager.AddPatient(dto);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("First name is required", result.Errors);
        _patientRepositoryMock.Verify(r => r.Create(It.IsAny<Patient>()), Times.Never);
    }

    
    [Fact]
    public void DeletingPatient_WithValidId_ReturnsSuccess_AndDeletesPatient()
    {
        // Arrange
        var id = Guid.NewGuid();

        var existingPatient = new Patient("Jane", "Smith", "2000 Antwerpen") { Id = id };

        _patientRepositoryMock
            .Setup(r => r.Read(id))
            .Returns(existingPatient);

        Patient? deletedPatient = null;
        _patientRepositoryMock
            .Setup(r => r.Delete(It.IsAny<Patient>()))
            .Callback<Patient>(p => deletedPatient = p);

        // Act
        var result = _patientManager.DeletePatient(id);

        // Assert
        Assert.True(result.Success);
        _patientRepositoryMock.Verify(r => r.Read(id), Times.Once);
        _patientRepositoryMock.Verify(r => r.Delete(existingPatient), Times.Once);
        Assert.Same(existingPatient, deletedPatient);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void DeletingPatient_WithInvalidId_ReturnsError()
    {
        // Arrange
        var id = Guid.NewGuid();
        _patientRepositoryMock
            .Setup(r => r.Read(id))
            .Returns((Patient?)null);

        // Act
        var result = _patientManager.DeletePatient(id);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Patient does not exist.", result.Errors);
        _patientRepositoryMock.Verify(r => r.Read(id), Times.Once);
        _patientRepositoryMock.Verify(r => r.Delete(It.IsAny<Patient>()), Times.Never);
    }
}
