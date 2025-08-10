using Chipsoft.Assignments.EPD.BLL.Dto;
using Chipsoft.Assignments.EPD.BLL.Managers;
using Chipsoft.Assignments.EPD.DAL;
using Chipsoft.Assignments.EPD.Domain;
using Moq;

namespace Chipsoft.Assignments.EPD.Tests;

public class PhysicianManagerUnitTests
{
    private readonly Mock<IPhysicianRepository> _physicianRepositoryMock;
    private readonly IPhysicianManager _physicianManager;

    public PhysicianManagerUnitTests()
    {
        _physicianRepositoryMock = new Mock<IPhysicianRepository>();
        _physicianManager = new PhysicianManager(_physicianRepositoryMock.Object);
    }

    [Fact]
    public void AddingPhysician_WithValidData_ReturnsSuccess_AndAddsPhysician()
    {
        // Arrange
        var dto = new AddPhysicianDto("John", "Doe", "Cardiology");
        Physician? createdPhysician = null;

        _physicianRepositoryMock
            .Setup(r => r.Create(It.IsAny<Physician>()))
            .Callback<Physician>(p => createdPhysician = p);

        // Act
        var result = _physicianManager.AddPhysician(dto);

        // Assert
        Assert.True(result.Success);
        _physicianRepositoryMock.Verify(r => r.Create(It.IsAny<Physician>()), Times.Once);
        Assert.NotNull(createdPhysician);
        Assert.Equal(dto.FirstName, createdPhysician!.FirstName);
        Assert.Equal(dto.LastName, createdPhysician.LastName);
        Assert.Equal(dto.Department, createdPhysician.Department);
    }
    
    [Fact]
    public void AddingPhysician_WithInvalidData_ReturnsError()
    {
        // Arrange
        var dto = new AddPhysicianDto("John", "Doe", ""); // Invalid: Department is empty

        // Act
        var result = _physicianManager.AddPhysician(dto);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Department is required", result.Errors);
        _physicianRepositoryMock.Verify(r => r.Create(It.IsAny<Physician>()), Times.Never);
    }

    [Fact]
    public void DeletingPhysician_WithValidId_ReturnsSuccess_AndDeletesPhysician()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingPhysician = new Physician("Jane", "Smith", "Oncology") { Id = id };

        _physicianRepositoryMock
            .Setup(r => r.Read(id))
            .Returns(existingPhysician);

        Physician? deletedPhysician = null;
        _physicianRepositoryMock
            .Setup(r => r.Delete(It.IsAny<Physician>()))
            .Callback<Physician>(p => deletedPhysician = p);

        // Act
        var result = _physicianManager.DeletePhysician(id);

        // Assert
        Assert.True(result.Success);
        _physicianRepositoryMock.Verify(r => r.Read(id), Times.Once);
        _physicianRepositoryMock.Verify(r => r.Delete(existingPhysician), Times.Once);
        Assert.Same(existingPhysician, deletedPhysician);
        Assert.Empty(result.Errors);
    }
    
    [Fact]
    public void DeletingPhysician_WithInvalidId_ReturnsError()
    {
        // Arrange
        var id = Guid.NewGuid();
        _physicianRepositoryMock
            .Setup(r => r.Read(id))
            .Returns((Physician?)null);
        
        // Act
        var result = _physicianManager.DeletePhysician(id);
        
        // Assert
        Assert.False(result.Success);
        Assert.Contains("Physician does not exist.", result.Errors);
        _physicianRepositoryMock.Verify(r => r.Read(id), Times.Once);
        _physicianRepositoryMock.Verify(r => r.Delete(It.IsAny<Physician>()), Times.Never);
    }

}