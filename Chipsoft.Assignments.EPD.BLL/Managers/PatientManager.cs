using Chipsoft.Assignments.EPD.BLL.Dto;
using Chipsoft.Assignments.EPD.BLL.Validators;
using Chipsoft.Assignments.EPD.DAL;
using Chipsoft.Assignments.EPD.Domain;

namespace Chipsoft.Assignments.EPD.BLL.Managers;

public class PatientManager(IPatientRepository repository) : IPatientManager
{
    public IEnumerable<GetPatientDto> GetAllPatients()
    {
        return repository.ReadAll().Select(GetPatientDto.FromPatient);
    }

    public OperationResult AddPatient(AddPatientDto patientDto)
    {
        var patient = patientDto.ToPatient();
        var validator = new PatientValidator();

        var result = validator.Validate(patient);

        if (result.IsValid)
        {
            repository.Create(patient);
        }

        return result.IsValid
            ? new OperationResult { Success = true }
            : new OperationResult { Success = false, Errors = result.Errors.Select(x => x.ErrorMessage).ToList() };
    }

    public OperationResult DeletePatient(Guid id)
    {
        var patient = repository.Read(id);

        if (patient != null)
        {
            repository.Delete(patient);
        }

        return patient == null
            ? new OperationResult { Success = false, Errors = ["Patient does not exist."] }
            : new OperationResult { Success = true };
    }
}