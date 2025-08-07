using Chipsoft.Assignments.EPD.Domain;

namespace Chipsoft.Assignments.EPD.BLL.Dto;

public record AddPatientDto(string FirstName, string LastName, string Address);

// Extension methods for use in BLL
internal static class AddPatientDtoExtensions
{
    internal static Patient ToPatient(this AddPatientDto dto)
    {
        return new Patient(dto.FirstName, dto.LastName, dto.Address);
    }
}