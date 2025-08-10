using Chipsoft.Assignments.EPD.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chipsoft.Assignments.EPD.DAL.EF;

public class EpdDbContext : DbContext
{
    public EpdDbContext()
    {
    }

    public EpdDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=epd.db")
                .UseLoggerFactory(LoggerFactory.Create(_ => { }));
        }
    }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Physician> Physicians { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Physician)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}