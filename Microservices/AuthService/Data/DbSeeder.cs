using AuthService.Models.Entities;
using AuthService.Models.Enums;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(HospitalDbContext context)
        {
            try
            {
                // Ensure database exists and apply migrations
                await context.Database.EnsureCreatedAsync();
                
                // Check if data already exists
                var patientCount = await context.Patients.CountAsync();
                var doctorCount = await context.Doctors.CountAsync();
                
                if (patientCount > 0 || doctorCount > 0)
                    return; // Data already seeded

                // Seed Patients
                var patients = new List<Patient>
                {
                    new Patient
                    {
                        Name = "John Smith",
                        Email = "john.smith@email.com",
                        Age = 35,
                        Gender = "Male",
                        Address = "123 Main St, New York, NY",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                new Patient
                {
                    Name = "Sarah Johnson",
                    Email = "sarah.johnson@email.com",
                    Age = 28,
                    Gender = "Female",
                    Address = "456 Oak Ave, Los Angeles, CA",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Patient
                {
                    Name = "Michael Brown",
                    Email = "michael.brown@email.com",
                    Age = 42,
                    Gender = "Male",
                    Address = "789 Pine Rd, Chicago, IL",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Patient
                {
                    Name = "Emily Davis",
                    Email = "emily.davis@email.com",
                    Age = 31,
                    Gender = "Female",
                    Address = "321 Elm St, Houston, TX",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Patient
                {
                    Name = "Robert Wilson",
                    Email = "robert.wilson@email.com",
                    Age = 55,
                    Gender = "Male",
                    Address = "654 Maple Dr, Phoenix, AZ",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
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
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Doctor
                {
                    Name = "Dr. Lisa Martinez",
                    Specialization = "Pediatrics",
                    Email = "dr.martinez@hospital.com",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Doctor
                {
                    Name = "Dr. David Thompson",
                    Specialization = "Orthopedics",
                    Email = "dr.thompson@hospital.com",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Doctor
                {
                    Name = "Dr. Jennifer Lee",
                    Specialization = "Dermatology",
                    Email = "dr.lee@hospital.com",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Doctor
                {
                    Name = "Dr. Christopher Garcia",
                    Specialization = "Neurology",
                    Email = "dr.garcia@hospital.com",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Doctor
                {
                    Name = "Dr. Amanda White",
                    Specialization = "Internal Medicine",
                    Email = "dr.white@hospital.com",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

                // Add to context
                await context.Patients.AddRangeAsync(patients);
                await context.Doctors.AddRangeAsync(doctors);

                // Save changes to get IDs
                await context.SaveChangesAsync();

                // Seed Users with hashed passwords
                var users = new List<User>
            {
                new User
                {
                    Username = "admin",
                    Email = "admin@hospital.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = UserRole.Admin,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Username = "dr.anderson",
                    Email = "dr.anderson@hospital.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("doctor123"),
                    Role = UserRole.Doctor,
                    DoctorId = doctors[0].Id,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Username = "dr.martinez",
                    Email = "dr.martinez@hospital.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("doctor123"),
                    Role = UserRole.Doctor,
                    DoctorId = doctors[1].Id,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Username = "john.smith",
                    Email = "john.smith@email.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("patient123"),
                    Role = UserRole.Patient,
                    PatientId = patients[0].Id,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Username = "sarah.johnson",
                    Email = "sarah.johnson@email.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("patient123"),
                    Role = UserRole.Patient,
                    PatientId = patients[1].Id,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

                await context.Users.AddRangeAsync(users);

                // Seed Sample Appointments
                var appointments = new List<Appointment>
            {
                new Appointment
                {
                    PatientId = patients[0].Id,
                    DoctorId = doctors[0].Id,
                    AppointmentDate = DateTime.Today.AddDays(1).AddHours(10), // Tomorrow at 10 AM
                    Duration = 30,
                    Status = AppointmentStatus.Scheduled,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Appointment
                {
                    PatientId = patients[1].Id,
                    DoctorId = doctors[1].Id,
                    AppointmentDate = DateTime.Today.AddDays(2).AddHours(14), // Day after tomorrow at 2 PM
                    Duration = 45,
                    Status = AppointmentStatus.Scheduled,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Appointment
                {
                    PatientId = patients[2].Id,
                    DoctorId = doctors[2].Id,
                    AppointmentDate = DateTime.Today.AddDays(3).AddHours(9), // 3 days from now at 9 AM
                    Duration = 60,
                    Status = AppointmentStatus.Scheduled,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                }
            };

                await context.Appointments.AddRangeAsync(appointments);

                // Final save
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to seed database", ex);
            }
        }
    }
}
