using Chipsoft.Assignments.EPD.Domain;

namespace Chipsoft.Assignments.EPD.BLL.Dto;

public record AddPhysicianDto(string FirstName, string LastName, string Department)
{
    internal Physician ToPhysician()
    {
        return new Physician(FirstName, LastName, Department);
    }
}