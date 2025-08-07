using Chipsoft.Assignments.EPD.BLL.Dto;
using Chipsoft.Assignments.EPD.BLL.Validators;
using Chipsoft.Assignments.EPD.DAL;

namespace Chipsoft.Assignments.EPD.BLL.Managers;

public class PatientManager(IPatientRepository repository) : IPatientManager
{
    public OperationResult AddPatient(AddPatientDto patientDto)
    {
        var patient = patientDto.ToPatient();
        var validator = new PatientValidator();

        var result = validator.Validate(patient);

        if (result.IsValid)
        {
            repository.Create(patient);
        }

        return result.IsValid ? 
            new OperationResult { Success = true } : 
            new OperationResult { Success = false, Errors = result.Errors.Select(x => x.ErrorMessage).ToList() };
    }
}