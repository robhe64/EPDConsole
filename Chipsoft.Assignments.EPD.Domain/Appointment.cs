using System.ComponentModel.DataAnnotations;

namespace Chipsoft.Assignments.EPD.Domain;

public class Appointment
{
    [Key]
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Patient Patient { get; set; } = null!;
    public Physician Physician { get; set; } = null!;

    // parameterless constructor for EF
    private Appointment() { }

    public Appointment(DateTime startTime, DateTime endTime, Patient patient, Physician physician)
    {
        StartTime = startTime;
        EndTime = endTime;
        Patient = patient;
        Physician = physician;
    }
}

