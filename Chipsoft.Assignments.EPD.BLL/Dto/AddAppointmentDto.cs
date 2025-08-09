namespace Chipsoft.Assignments.EPD.BLL.Dto;

public record AddAppointmentDto(Guid PatientId, Guid PhysicianId, DateTime DateTime, int Duration);