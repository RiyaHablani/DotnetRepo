using Microsoft.EntityFrameworkCore;
using MedicalRecordsService.Models.Entities;

namespace MedicalRecordsService.Data
{
    public class MedicalRecordsDbContext : DbContext
    {
        public MedicalRecordsDbContext(DbContextOptions<MedicalRecordsDbContext> options) : base(options)
        {
        }

        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<LabReport> LabReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure MedicalRecord entity
            modelBuilder.Entity<MedicalRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Diagnosis).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Symptoms).HasMaxLength(1000);
                entity.Property(e => e.Treatment).HasMaxLength(1000);
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.Property(e => e.RecordDate).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                entity.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);
                
                // Configure indexes
                entity.HasIndex(e => e.PatientId).HasDatabaseName("IX_MedicalRecords_PatientId");
                entity.HasIndex(e => e.DoctorId).HasDatabaseName("IX_MedicalRecords_DoctorId");
                entity.HasIndex(e => e.RecordDate).HasDatabaseName("IX_MedicalRecords_RecordDate");
            });

            // Configure LabReport entity
            modelBuilder.Entity<LabReport>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TestName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.TestDescription).HasMaxLength(500);
                entity.Property(e => e.Results).HasMaxLength(1000);
                entity.Property(e => e.Status).HasMaxLength(100).HasDefaultValue("Pending");
                entity.Property(e => e.Notes).HasMaxLength(500);
                entity.Property(e => e.TestDate).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                entity.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);
                
                // Configure indexes
                entity.HasIndex(e => e.PatientId).HasDatabaseName("IX_LabReports_PatientId");
                entity.HasIndex(e => e.DoctorId).HasDatabaseName("IX_LabReports_DoctorId");
                entity.HasIndex(e => e.TestDate).HasDatabaseName("IX_LabReports_TestDate");
                entity.HasIndex(e => e.Status).HasDatabaseName("IX_LabReports_Status");
            });
        }
    }
}
