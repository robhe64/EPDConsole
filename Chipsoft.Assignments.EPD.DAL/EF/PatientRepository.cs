using Chipsoft.Assignments.EPD.Domain;

namespace Chipsoft.Assignments.EPD.DAL.EF;

public class PatientRepository(EpdDbContext context) : Repository<Patient>(context), IPatientRepository;