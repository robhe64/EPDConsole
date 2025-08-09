using Chipsoft.Assignments.EPD.BLL.Dto;

namespace Chipsoft.Assignments.EPD.BLL.Managers;

public interface IPatientManager
{
    OperationResult AddPatient(AddPatientDto patientDto);
    IEnumerable<GetPatientDto> GetAllPatients();
    OperationResult DeletePatient(Guid id);
}