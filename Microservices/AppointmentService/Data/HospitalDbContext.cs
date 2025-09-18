using Microsoft.EntityFrameworkCore;
using AppointmentService.Models.Entities;

namespace AppointmentService.Data
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Appointment entity
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AppointmentDate).IsRequired();
                entity.Property(e => e.Duration).IsRequired().HasDefaultValue(30);
                entity.Property(e => e.Status).IsRequired().HasDefaultValue("Scheduled");
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                entity.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);
                
                // Configure indexes
                entity.HasIndex(e => new { e.DoctorId, e.AppointmentDate })
                      .HasDatabaseName("IX_Appointments_DoctorId_AppointmentDate");
                entity.HasIndex(e => e.PatientId)
                      .HasDatabaseName("IX_Appointments_PatientId");
                entity.HasIndex(e => e.Status)
                      .HasDatabaseName("IX_Appointments_Status");
            });
        }
    }
}
