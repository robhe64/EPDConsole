namespace Chipsoft.Assignments.EPD.Domain;

public class Patient(string firstName, string lastName, string address) : Person(firstName, lastName)
{
    // Could be split off into City, Street, PostalCode... but combined into a single string for simplicity
    public string Address { get; set; } = address;
}