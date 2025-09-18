using MedicalRecordsService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalRecordsService.Data
{
    public static class DbSeeder
    {
        public static async Task SeedDataAsync(MedicalRecordsDbContext context)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Check if data already exists
            if (await context.MedicalRecords.AnyAsync() || await context.LabReports.AnyAsync())
            {
                return; // Data already seeded
            }

            var now = DateTime.UtcNow;

            // Seed Medical Records
            var medicalRecords = new List<MedicalRecord>
            {
                new MedicalRecord
                {
                    PatientId = 1,
                    DoctorId = 1,
                    Diagnosis = "Hypertension",
                    Symptoms = "High blood pressure, headaches, dizziness",
                    Treatment = "ACE inhibitor medication, lifestyle changes, regular monitoring",
                    Notes = "Patient needs to monitor blood pressure daily",
                    RecordDate = now.AddDays(-30),
                    CreatedAt = now.AddDays(-30),
                    UpdatedAt = now.AddDays(-30),
                    IsDeleted = false
                },
                new MedicalRecord
                {
                    PatientId = 1,
                    DoctorId = 2,
                    Diagnosis = "Type 2 Diabetes",
                    Symptoms = "Increased thirst, frequent urination, fatigue",
                    Treatment = "Metformin, dietary changes, regular blood sugar monitoring",
                    Notes = "Patient needs to follow diabetic diet and exercise regularly",
                    RecordDate = now.AddDays(-25),
                    CreatedAt = now.AddDays(-25),
                    UpdatedAt = now.AddDays(-25),
                    IsDeleted = false
                },
                new MedicalRecord
                {
                    PatientId = 2,
                    DoctorId = 1,
                    Diagnosis = "Common Cold",
                    Symptoms = "Runny nose, cough, sore throat, mild fever",
                    Treatment = "Rest, fluids, over-the-counter cold medication",
                    Notes = "Viral infection, should resolve in 7-10 days",
                    RecordDate = now.AddDays(-15),
                    CreatedAt = now.AddDays(-15),
                    UpdatedAt = now.AddDays(-15),
                    IsDeleted = false
                },
                new MedicalRecord
                {
                    PatientId = 2,
                    DoctorId = 3,
                    Diagnosis = "Allergic Rhinitis",
                    Symptoms = "Sneezing, nasal congestion, itchy eyes",
                    Treatment = "Antihistamines, nasal spray, allergen avoidance",
                    Notes = "Seasonal allergies, patient should avoid pollen exposure",
                    RecordDate = now.AddDays(-10),
                    CreatedAt = now.AddDays(-10),
                    UpdatedAt = now.AddDays(-10),
                    IsDeleted = false
                },
                new MedicalRecord
                {
                    PatientId = 3,
                    DoctorId = 2,
                    Diagnosis = "Gastroenteritis",
                    Symptoms = "Nausea, vomiting, diarrhea, abdominal pain",
                    Treatment = "Fluid replacement, bland diet, anti-nausea medication",
                    Notes = "Food poisoning suspected, patient recovering well",
                    RecordDate = now.AddDays(-5),
                    CreatedAt = now.AddDays(-5),
                    UpdatedAt = now.AddDays(-5),
                    IsDeleted = false
                },
                new MedicalRecord
                {
                    PatientId = 4,
                    DoctorId = 1,
                    Diagnosis = "Migraine",
                    Symptoms = "Severe headache, nausea, sensitivity to light",
                    Treatment = "Triptan medication, rest in dark room, stress management",
                    Notes = "Chronic condition, patient has migraine diary",
                    RecordDate = now.AddDays(-20),
                    CreatedAt = now.AddDays(-20),
                    UpdatedAt = now.AddDays(-20),
                    IsDeleted = false
                }
            };

            await context.MedicalRecords.AddRangeAsync(medicalRecords);

            // Seed Lab Reports
            var labReports = new List<LabReport>
            {
                new LabReport
                {
                    PatientId = 1,
                    DoctorId = 1,
                    TestName = "Complete Blood Count (CBC)",
                    TestDescription = "Full blood count including red and white blood cells, platelets",
                    Results = "All values within normal range",
                    Status = "Completed",
                    Notes = "Normal CBC results",
                    TestDate = now.AddDays(-30),
                    CompletedDate = now.AddDays(-29),
                    CreatedAt = now.AddDays(-30),
                    UpdatedAt = now.AddDays(-29),
                    IsDeleted = false
                },
                new LabReport
                {
                    PatientId = 1,
                    DoctorId = 2,
                    TestName = "Hemoglobin A1C",
                    TestDescription = "Average blood sugar levels over 2-3 months",
                    Results = "A1C: 7.2% (elevated, indicates diabetes)",
                    Status = "Completed",
                    Notes = "Confirms Type 2 diabetes diagnosis",
                    TestDate = now.AddDays(-25),
                    CompletedDate = now.AddDays(-24),
                    CreatedAt = now.AddDays(-25),
                    UpdatedAt = now.AddDays(-24),
                    IsDeleted = false
                },
                new LabReport
                {
                    PatientId = 1,
                    DoctorId = 2,
                    TestName = "Lipid Panel",
                    TestDescription = "Cholesterol and triglyceride levels",
                    Results = "Total Cholesterol: 220 mg/dL, LDL: 140 mg/dL, HDL: 45 mg/dL",
                    Status = "Completed",
                    Notes = "Elevated cholesterol, needs dietary intervention",
                    TestDate = now.AddDays(-20),
                    CompletedDate = now.AddDays(-19),
                    CreatedAt = now.AddDays(-20),
                    UpdatedAt = now.AddDays(-19),
                    IsDeleted = false
                },
                new LabReport
                {
                    PatientId = 2,
                    DoctorId = 1,
                    TestName = "Rapid Strep Test",
                    TestDescription = "Test for streptococcal throat infection",
                    Results = "Negative for strep throat",
                    Status = "Completed",
                    Notes = "Viral infection confirmed",
                    TestDate = now.AddDays(-15),
                    CompletedDate = now.AddDays(-14),
                    CreatedAt = now.AddDays(-15),
                    UpdatedAt = now.AddDays(-14),
                    IsDeleted = false
                },
                new LabReport
                {
                    PatientId = 2,
                    DoctorId = 3,
                    TestName = "Allergy Panel",
                    TestDescription = "Blood test for common allergens",
                    Results = "Positive for pollen, dust mites, and pet dander",
                    Status = "Completed",
                    Notes = "Multiple allergies identified",
                    TestDate = now.AddDays(-10),
                    CompletedDate = now.AddDays(-9),
                    CreatedAt = now.AddDays(-10),
                    UpdatedAt = now.AddDays(-9),
                    IsDeleted = false
                },
                new LabReport
                {
                    PatientId = 3,
                    DoctorId = 2,
                    TestName = "Stool Culture",
                    TestDescription = "Test for bacterial pathogens in stool",
                    Results = "No pathogenic bacteria found",
                    Status = "Completed",
                    Notes = "Viral gastroenteritis confirmed",
                    TestDate = now.AddDays(-5),
                    CompletedDate = now.AddDays(-4),
                    CreatedAt = now.AddDays(-5),
                    UpdatedAt = now.AddDays(-4),
                    IsDeleted = false
                },
                new LabReport
                {
                    PatientId = 4,
                    DoctorId = 1,
                    TestName = "MRI Brain Scan",
                    TestDescription = "Magnetic resonance imaging of the brain",
                    Results = "No structural abnormalities found",
                    Status = "Completed",
                    Notes = "Normal brain MRI, migraine likely tension-related",
                    TestDate = now.AddDays(-20),
                    CompletedDate = now.AddDays(-18),
                    CreatedAt = now.AddDays(-20),
                    UpdatedAt = now.AddDays(-18),
                    IsDeleted = false
                },
                new LabReport
                {
                    PatientId = 5,
                    DoctorId = 3,
                    TestName = "Thyroid Function Test",
                    TestDescription = "TSH, T3, T4 levels to assess thyroid function",
                    Results = "TSH: 4.5 mIU/L (elevated), T4: 0.8 ng/dL (low)",
                    Status = "Completed",
                    Notes = "Hypothyroidism detected, needs treatment",
                    TestDate = now.AddDays(-12),
                    CompletedDate = now.AddDays(-11),
                    CreatedAt = now.AddDays(-12),
                    UpdatedAt = now.AddDays(-11),
                    IsDeleted = false
                },
                new LabReport
                {
                    PatientId = 6,
                    DoctorId = 2,
                    TestName = "Urinalysis",
                    TestDescription = "Complete urine analysis",
                    Results = "Normal urinalysis, no signs of infection",
                    Status = "Completed",
                    Notes = "Routine checkup - normal results",
                    TestDate = now.AddDays(-8),
                    CompletedDate = now.AddDays(-7),
                    CreatedAt = now.AddDays(-8),
                    UpdatedAt = now.AddDays(-7),
                    IsDeleted = false
                },
                new LabReport
                {
                    PatientId = 7,
                    DoctorId = 1,
                    TestName = "Chest X-Ray",
                    TestDescription = "Radiographic examination of the chest",
                    Results = "Clear lung fields, no abnormalities",
                    Status = "Completed",
                    Notes = "Normal chest X-ray",
                    TestDate = now.AddDays(-3),
                    CompletedDate = now.AddDays(-2),
                    CreatedAt = now.AddDays(-3),
                    UpdatedAt = now.AddDays(-2),
                    IsDeleted = false
                }
            };

            await context.LabReports.AddRangeAsync(labReports);

            // Save all changes
            await context.SaveChangesAsync();
        }
    }
}
