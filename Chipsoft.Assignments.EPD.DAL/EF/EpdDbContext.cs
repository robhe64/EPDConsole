using Chipsoft.Assignments.EPD.Domain;
using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPD.DAL.EF
{
    public class EpdDbContext : DbContext
    {
        // The following configures EF to create a Sqlite database file in the
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=epd.db");
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Physician> Physicians { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
