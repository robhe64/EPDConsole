using Chipsoft.Assignments.EPD.Domain;

namespace Chipsoft.Assignments.EPD.DAL.EF;

public class PhysicianRepository(EpdDbContext context) : Repository<Physician>(context), IPhysicianRepository;