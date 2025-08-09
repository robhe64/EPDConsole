using Chipsoft.Assignments.EPD.Domain;

namespace Chipsoft.Assignments.EPD.BLL.Dto;

public record GetPatientDto(Guid Id, string FirstName, string LastName, string Address) 
{
    internal static GetPatientDto FromPatient(Patient patient)
    {
        return new GetPatientDto(patient.Id, patient.FirstName, patient.LastName, patient.Address);
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName}, {Address}";
    }
}