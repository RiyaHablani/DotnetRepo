using Microsoft.EntityFrameworkCore;
using PharmacyService.Models.Entities;

namespace PharmacyService.Data
{
    public class PharmacyDbContext : DbContext
    {
        public PharmacyDbContext(DbContextOptions<PharmacyDbContext> options) : base(options)
        {
        }

        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PatientMedicine> PatientMedicines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Medicine entity
            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(200);
                entity.Property(e => e.DiseaseType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.ExpiryDate).IsRequired();
                entity.Property(e => e.Manufacturer).HasMaxLength(100);
                entity.Property(e => e.DosageForm).HasMaxLength(50);
                entity.Property(e => e.DosageStrength).HasMaxLength(100);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                entity.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);
                
                // Configure indexes
                entity.HasIndex(e => e.Name).HasDatabaseName("IX_Medicines_Name");
                entity.HasIndex(e => e.DiseaseType).HasDatabaseName("IX_Medicines_DiseaseType");
                entity.HasIndex(e => e.ExpiryDate).HasDatabaseName("IX_Medicines_ExpiryDate");
                entity.HasIndex(e => e.IsDeleted).HasDatabaseName("IX_Medicines_IsDeleted");
            });

            // Configure Prescription entity
            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PatientId).IsRequired();
                entity.Property(e => e.DoctorId).IsRequired();
                entity.Property(e => e.PrescriptionDate).IsRequired();
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");
                entity.Property(e => e.FilledDate).IsRequired(false);
                entity.Property(e => e.FilledBy).HasMaxLength(100);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                entity.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);
                
                // Configure indexes
                entity.HasIndex(e => e.PatientId).HasDatabaseName("IX_Prescriptions_PatientId");
                entity.HasIndex(e => e.DoctorId).HasDatabaseName("IX_Prescriptions_DoctorId");
                entity.HasIndex(e => e.PrescriptionDate).HasDatabaseName("IX_Prescriptions_PrescriptionDate");
                entity.HasIndex(e => e.Status).HasDatabaseName("IX_Prescriptions_Status");
                entity.HasIndex(e => e.IsDeleted).HasDatabaseName("IX_Prescriptions_IsDeleted");
            });

            // Configure PatientMedicine entity
            modelBuilder.Entity<PatientMedicine>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PrescriptionId).IsRequired();
                entity.Property(e => e.MedicineId).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.Instructions).HasMaxLength(200);
                entity.Property(e => e.Dosage).HasMaxLength(100);
                entity.Property(e => e.Frequency).HasMaxLength(50);
                entity.Property(e => e.Duration).HasMaxLength(50);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                entity.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);
                
                // Configure foreign key relationships
                entity.HasOne(e => e.Prescription)
                    .WithMany(p => p.PatientMedicines)
                    .HasForeignKey(e => e.PrescriptionId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Medicine)
                    .WithMany()
                    .HasForeignKey(e => e.MedicineId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                // Configure indexes
                entity.HasIndex(e => e.PrescriptionId).HasDatabaseName("IX_PatientMedicines_PrescriptionId");
                entity.HasIndex(e => e.MedicineId).HasDatabaseName("IX_PatientMedicines_MedicineId");
                entity.HasIndex(e => e.IsDeleted).HasDatabaseName("IX_PatientMedicines_IsDeleted");
            });
        }
    }
}
