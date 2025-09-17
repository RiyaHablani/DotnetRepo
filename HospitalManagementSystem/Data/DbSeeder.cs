using HospitalManagementSystem.Models.Entities;

namespace HospitalManagementSystem.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(HospitalDbContext context)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Check if data already exists
            if (context.Patients.Any() || context.Doctors.Any())
                return; // Data already seeded

            // Seed Patients
            var patients = new List<Patient>
            {
                new Patient
                {
                    Name = "John Smith",
                    Age = 35,
                    Gender = "Male",
                    Address = "123 Main St, New York, NY",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Patient
                {
                    Name = "Sarah Johnson",
                    Age = 28,
                    Gender = "Female",
                    Address = "456 Oak Ave, Los Angeles, CA",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Patient
                {
                    Name = "Michael Brown",
                    Age = 42,
                    Gender = "Male",
                    Address = "789 Pine Rd, Chicago, IL",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Patient
                {
                    Name = "Emily Davis",
                    Age = 31,
                    Gender = "Female",
                    Address = "321 Elm St, Houston, TX",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Patient
                {
                    Name = "Robert Wilson",
                    Age = 55,
                    Gender = "Male",
                    Address = "654 Maple Dr, Phoenix, AZ",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                }
            };

            // Seed Doctors
            var doctors = new List<Doctor>
            {
                new Doctor
                {
                    Name = "Dr. James Anderson",
                    Specialization = "Cardiology",
                    Email = "dr.anderson@hospital.com",
                    IsActive = true
                },
                new Doctor
                {
                    Name = "Dr. Lisa Martinez",
                    Specialization = "Pediatrics",
                    Email = "dr.martinez@hospital.com",
                    IsActive = true
                },
                new Doctor
                {
                    Name = "Dr. David Thompson",
                    Specialization = "Orthopedics",
                    Email = "dr.thompson@hospital.com",
                    IsActive = true
                },
                new Doctor
                {
                    Name = "Dr. Jennifer Lee",
                    Specialization = "Dermatology",
                    Email = "dr.lee@hospital.com",
                    IsActive = true
                },
                new Doctor
                {
                    Name = "Dr. Christopher Garcia",
                    Specialization = "Neurology",
                    Email = "dr.garcia@hospital.com",
                    IsActive = true
                },
                new Doctor
                {
                    Name = "Dr. Amanda White",
                    Specialization = "Internal Medicine",
                    Email = "dr.white@hospital.com",
                    IsActive = true
                }
            };

            // Add to context
            await context.Patients.AddRangeAsync(patients);
            await context.Doctors.AddRangeAsync(doctors);

            // Save changes
            await context.SaveChangesAsync();
        }
    }
}
