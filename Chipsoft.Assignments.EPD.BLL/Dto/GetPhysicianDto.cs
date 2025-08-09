using Chipsoft.Assignments.EPD.Domain;

namespace Chipsoft.Assignments.EPD.BLL.Dto;

public record GetPhysicianDto(Guid Id, string FirstName, string LastName, string Department)
{
    internal static GetPhysicianDto FromPhysician(Physician physician)
    {
        return new GetPhysicianDto(physician.Id, physician.FirstName, physician.LastName, physician.Department);
    }
    
    public override string ToString()
    {
        return $"{FirstName} {LastName}, {Department}";
    }
}