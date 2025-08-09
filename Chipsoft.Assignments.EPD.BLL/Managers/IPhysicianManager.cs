using Chipsoft.Assignments.EPD.BLL.Dto;

namespace Chipsoft.Assignments.EPD.BLL.Managers;

public interface IPhysicianManager
{
    OperationResult AddPhysician(AddPhysicianDto physicianDto);
    
    IEnumerable<GetPhysicianDto> GetAllPhysicians();
    
    OperationResult DeletePhysician(Guid id);
}