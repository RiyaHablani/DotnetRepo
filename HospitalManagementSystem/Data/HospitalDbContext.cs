using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Models.Entities;

namespace HospitalManagementSystem.Data
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Patient entity
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Gender).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(250);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.IsDeleted).IsRequired();
                
                // Configure relationships
                entity.HasMany(p => p.Appointments)
                      .WithOne(a => a.Patient)
                      .HasForeignKey(a => a.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Doctor entity
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Specialization).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.IsActive).IsRequired();
                
                // Configure relationships
                entity.HasMany(d => d.Appointments)
                      .WithOne(a => a.Doctor)
                      .HasForeignKey(a => a.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Appointment entity
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AppointmentDate).IsRequired();
                entity.Property(e => e.Duration).IsRequired().HasDefaultValue(30);
                entity.Property(e => e.Status).IsRequired().HasDefaultValue(AppointmentStatus.Scheduled);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                entity.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);
                
                // Configure relationships
                entity.HasOne(a => a.Patient)
                      .WithMany(p => p.Appointments)
                      .HasForeignKey(a => a.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(a => a.Doctor)
                      .WithMany(d => d.Appointments)
                      .HasForeignKey(a => a.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                // Configure indexes
                entity.HasIndex(e => new { e.DoctorId, e.AppointmentDate })
                      .HasDatabaseName("IX_Appointments_DoctorId_AppointmentDate");
                entity.HasIndex(e => e.PatientId)
                      .HasDatabaseName("IX_Appointments_PatientId");
                entity.HasIndex(e => e.Status)
                      .HasDatabaseName("IX_Appointments_Status");
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Role).IsRequired();
                entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                
                // Configure unique constraints
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                
                // Configure relationships
                entity.HasOne(u => u.Patient)
                      .WithOne(p => p.User)
                      .HasForeignKey<User>(u => u.PatientId)
                      .OnDelete(DeleteBehavior.SetNull);
                      
                entity.HasOne(u => u.Doctor)
                      .WithOne(d => d.User)
                      .HasForeignKey<User>(u => u.DoctorId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
