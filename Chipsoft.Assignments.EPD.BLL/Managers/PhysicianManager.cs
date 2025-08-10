using Chipsoft.Assignments.EPD.BLL.Dto;
using Chipsoft.Assignments.EPD.BLL.Validators;
using Chipsoft.Assignments.EPD.DAL;

namespace Chipsoft.Assignments.EPD.BLL.Managers;

public class PhysicianManager(IPhysicianRepository repository) : IPhysicianManager
{
    public OperationResult AddPhysician(AddPhysicianDto physicianDto)
    {
        var physician = physicianDto.ToPhysician();
        var validator = new PhysicianValidator();
        
        var result = validator.Validate(physician);
        
        if (result.IsValid)
        {
            repository.Create(physician);
        }

        return result.IsValid
            ? new OperationResult { Success = true }
            : new OperationResult { Success = false, Errors = result.Errors.Select(x => x.ErrorMessage).ToList() };
    }

    public IEnumerable<GetPhysicianDto> GetAllPhysicians()
    {
        return repository.ReadAll().Select(GetPhysicianDto.FromPhysician);
    }

    public OperationResult DeletePhysician(Guid id)
    {
        var physician = repository.Read(id);

        if (physician != null)
        {
            repository.Delete(physician);
        }

        return physician == null
            ? new OperationResult { Success = false, Errors = ["Physician does not exist."] }
            : new OperationResult { Success = true };
    }
}