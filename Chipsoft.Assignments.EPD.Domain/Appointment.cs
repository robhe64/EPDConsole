using System.ComponentModel.DataAnnotations;

namespace Chipsoft.Assignments.EPD.Domain;

public class Appointment
{
    [Key]
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Patient Patient { get; set; }
    public Physician Physician { get; set; }
}