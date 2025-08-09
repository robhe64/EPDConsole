namespace Chipsoft.Assignments.EPD.Domain;

public class Physician(string firstName, string lastName, string department) : Person(firstName, lastName)
{
    public string Department { get; init; } = department;
}