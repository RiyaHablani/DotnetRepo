using PharmacyService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace PharmacyService.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(PharmacyDbContext context)
        {
            try
            {
                // Test database connection first
                if (!await context.Database.CanConnectAsync())
                {
                    Console.WriteLine("⚠️ Cannot connect to database. Skipping seeding.");
                    return;
                }

                // Ensure database exists and apply migrations
                await context.Database.EnsureCreatedAsync();
                
                // Check if data already exists
                var medicineCount = await context.Medicines.CountAsync();
                var prescriptionCount = await context.Prescriptions.CountAsync();
                
                if (medicineCount > 0 || prescriptionCount > 0)
                    return; // Data already seeded

                // Seed Medicines
                var medicines = new List<Medicine>
                {
                    new Medicine
                    {
                        Name = "Paracetamol",
                        Description = "Pain reliever and fever reducer",
                        DiseaseType = "Pain Management",
                        Price = 5.50m,
                        Quantity = 100,
                        ExpiryDate = new DateTime(2025, 12, 31),
                        Manufacturer = "Generic Pharma",
                        DosageForm = "Tablet",
                        DosageStrength = "500mg",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new Medicine
                    {
                        Name = "Amoxicillin",
                        Description = "Antibiotic for bacterial infections",
                        DiseaseType = "Infections",
                        Price = 12.75m,
                        Quantity = 50,
                        ExpiryDate = new DateTime(2025, 6, 30),
                        Manufacturer = "MedCorp",
                        DosageForm = "Capsule",
                        DosageStrength = "250mg",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new Medicine
                    {
                        Name = "Insulin",
                        Description = "Diabetes management medication",
                        DiseaseType = "Diabetes",
                        Price = 45.00m,
                        Quantity = 25,
                        ExpiryDate = new DateTime(2025, 3, 31),
                        Manufacturer = "DiabeticCare",
                        DosageForm = "Injection",
                        DosageStrength = "100 units/ml",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new Medicine
                    {
                        Name = "Lisinopril",
                        Description = "Blood pressure medication",
                        DiseaseType = "Cardiovascular",
                        Price = 8.25m,
                        Quantity = 75,
                        ExpiryDate = new DateTime(2025, 9, 30),
                        Manufacturer = "HeartMed",
                        DosageForm = "Tablet",
                        DosageStrength = "10mg",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new Medicine
                    {
                        Name = "Metformin",
                        Description = "Type 2 diabetes medication",
                        DiseaseType = "Diabetes",
                        Price = 6.50m,
                        Quantity = 80,
                        ExpiryDate = new DateTime(2025, 8, 31),
                        Manufacturer = "DiabeticCare",
                        DosageForm = "Tablet",
                        DosageStrength = "500mg",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new Medicine
                    {
                        Name = "Aspirin",
                        Description = "Anti-inflammatory and blood thinner",
                        DiseaseType = "Cardiovascular",
                        Price = 3.25m,
                        Quantity = 120,
                        ExpiryDate = new DateTime(2025, 11, 30),
                        Manufacturer = "Generic Pharma",
                        DosageForm = "Tablet",
                        DosageStrength = "81mg",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new Medicine
                    {
                        Name = "Ibuprofen",
                        Description = "Anti-inflammatory pain reliever",
                        DiseaseType = "Pain Management",
                        Price = 4.75m,
                        Quantity = 90,
                        ExpiryDate = new DateTime(2025, 7, 31),
                        Manufacturer = "PainRelief Inc",
                        DosageForm = "Tablet",
                        DosageStrength = "200mg",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new Medicine
                    {
                        Name = "Omeprazole",
                        Description = "Acid reflux medication",
                        DiseaseType = "Digestive",
                        Price = 9.50m,
                        Quantity = 60,
                        ExpiryDate = new DateTime(2025, 10, 31),
                        Manufacturer = "Digestive Health",
                        DosageForm = "Capsule",
                        DosageStrength = "20mg",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new Medicine
                    {
                        Name = "Cetirizine",
                        Description = "Antihistamine for allergies",
                        DiseaseType = "Allergies",
                        Price = 7.25m,
                        Quantity = 85,
                        ExpiryDate = new DateTime(2025, 5, 31),
                        Manufacturer = "AllergyCare",
                        DosageForm = "Tablet",
                        DosageStrength = "10mg",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new Medicine
                    {
                        Name = "Simvastatin",
                        Description = "Cholesterol lowering medication",
                        DiseaseType = "Cardiovascular",
                        Price = 15.75m,
                        Quantity = 40,
                        ExpiryDate = new DateTime(2025, 4, 30),
                        Manufacturer = "HeartMed",
                        DosageForm = "Tablet",
                        DosageStrength = "20mg",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    }
                };

                await context.Medicines.AddRangeAsync(medicines);
                await context.SaveChangesAsync();

                // Seed Prescriptions
                var prescriptions = new List<Prescription>
                {
                    new Prescription
                    {
                        PatientId = 1,
                        DoctorId = 1,
                        PrescriptionDate = DateTime.UtcNow.AddDays(-2),
                        Notes = "Patient has high blood pressure, monitor regularly",
                        Status = "Pending",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new Prescription
                    {
                        PatientId = 2,
                        DoctorId = 2,
                        PrescriptionDate = DateTime.UtcNow.AddDays(-1),
                        Notes = "Diabetic patient, check blood sugar levels",
                        Status = "Filled",
                        FilledDate = DateTime.UtcNow.AddHours(-2),
                        FilledBy = "John Pharmacist",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new Prescription
                    {
                        PatientId = 3,
                        DoctorId = 1,
                        PrescriptionDate = DateTime.UtcNow,
                        Notes = "Post-surgery pain management",
                        Status = "Pending",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new Prescription
                    {
                        PatientId = 1,
                        DoctorId = 3,
                        PrescriptionDate = DateTime.UtcNow.AddDays(-3),
                        Notes = "Allergy management for seasonal allergies",
                        Status = "Filled",
                        FilledDate = DateTime.UtcNow.AddDays(-1),
                        FilledBy = "Jane Pharmacist",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    }
                };

                await context.Prescriptions.AddRangeAsync(prescriptions);
                await context.SaveChangesAsync();

                // Seed PatientMedicines
                var patientMedicines = new List<PatientMedicine>
                {
                    new PatientMedicine
                    {
                        PrescriptionId = 1,
                        MedicineId = 4, // Lisinopril
                        Quantity = 30,
                        Instructions = "Take with food",
                        Dosage = "1 tablet",
                        Frequency = "Once daily",
                        Duration = "30 days",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new PatientMedicine
                    {
                        PrescriptionId = 2,
                        MedicineId = 3, // Insulin
                        Quantity = 1,
                        Instructions = "Inject as directed",
                        Dosage = "1 vial",
                        Frequency = "As needed",
                        Duration = "30 days",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new PatientMedicine
                    {
                        PrescriptionId = 2,
                        MedicineId = 5, // Metformin
                        Quantity = 60,
                        Instructions = "Take with meals",
                        Dosage = "1 tablet",
                        Frequency = "Twice daily",
                        Duration = "30 days",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new PatientMedicine
                    {
                        PrescriptionId = 3,
                        MedicineId = 1, // Paracetamol
                        Quantity = 20,
                        Instructions = "Take as needed for pain",
                        Dosage = "1-2 tablets",
                        Frequency = "Every 6 hours",
                        Duration = "7 days",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new PatientMedicine
                    {
                        PrescriptionId = 3,
                        MedicineId = 7, // Ibuprofen
                        Quantity = 15,
                        Instructions = "Take with food to avoid stomach upset",
                        Dosage = "1 tablet",
                        Frequency = "Three times daily",
                        Duration = "5 days",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new PatientMedicine
                    {
                        PrescriptionId = 4,
                        MedicineId = 9, // Cetirizine
                        Quantity = 30,
                        Instructions = "Take at bedtime",
                        Dosage = "1 tablet",
                        Frequency = "Once daily",
                        Duration = "30 days",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    }
                };

                await context.PatientMedicines.AddRangeAsync(patientMedicines);
                await context.SaveChangesAsync();

                Console.WriteLine("✅ PharmacyService database seeded successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error seeding PharmacyService database: {ex.Message}");
                throw;
            }
        }
    }
}
