using Microsoft.EntityFrameworkCore;
using PatientService.Models.Entities;

namespace PatientService.Data
{
    public class PatientDbContext : DbContext
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }

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
            });
        }
    }
}
