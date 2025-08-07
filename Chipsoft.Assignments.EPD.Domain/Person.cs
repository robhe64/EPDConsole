using System.ComponentModel.DataAnnotations;

namespace Chipsoft.Assignments.EPD.Domain;

public abstract class Person(string firstName, string lastName)
{
    [Key]
    public Guid Id { get; set; }

    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    

}