using System.ComponentModel.DataAnnotations;

namespace Chipsoft.Assignments.EPD.Domain;

public class Appointment
{
    [Key]
    public Guid Id { get; set; }
}