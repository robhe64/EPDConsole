using Chipsoft.Assignments.EPD.Domain;

namespace Chipsoft.Assignments.EPD.BLL.Dto;

public record AddPatientDto(string FirstName, string LastName, string Address) 
{
    internal Patient ToPatient()
    {
        return new Patient(FirstName, LastName, Address);
    }
}