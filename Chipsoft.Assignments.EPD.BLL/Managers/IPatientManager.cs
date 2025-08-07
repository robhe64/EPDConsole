using Chipsoft.Assignments.EPD.BLL.Dto;
using Chipsoft.Assignments.EPD.Domain;

namespace Chipsoft.Assignments.EPD.BLL.Managers;

public interface IPatientManager
{
    public OperationResult AddPatient(AddPatientDto patientDto);
}