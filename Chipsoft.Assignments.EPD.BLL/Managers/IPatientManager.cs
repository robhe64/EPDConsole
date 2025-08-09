using Chipsoft.Assignments.EPD.BLL.Dto;
using Chipsoft.Assignments.EPD.Domain;

namespace Chipsoft.Assignments.EPD.BLL.Managers;

public interface IPatientManager
{
    OperationResult AddPatient(AddPatientDto patientDto);
    IEnumerable<GetPatientDto> GetAllPatients();
    OperationResult DeletePatient(Guid id);
}