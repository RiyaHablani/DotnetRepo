-- Pharmacy Service Database Setup Script
-- This script creates the necessary tables for the Pharmacy Service

USE HospitalManagementDB;

-- Create Medicines table
CREATE TABLE IF NOT EXISTS Medicines (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description VARCHAR(200),
    DiseaseType VARCHAR(50) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Quantity INT NOT NULL,
    ExpiryDate DATETIME NOT NULL,
    Manufacturer VARCHAR(100),
    DosageForm VARCHAR(50),
    DosageStrength VARCHAR(100),
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE,
    INDEX IX_Medicines_Name (Name),
    INDEX IX_Medicines_DiseaseType (DiseaseType),
    INDEX IX_Medicines_ExpiryDate (ExpiryDate),
    INDEX IX_Medicines_IsDeleted (IsDeleted)
);

-- Create Prescriptions table
CREATE TABLE IF NOT EXISTS Prescriptions (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PatientId INT NOT NULL,
    DoctorId INT NOT NULL,
    PrescriptionDate DATETIME NOT NULL,
    Notes TEXT,
    Status VARCHAR(50) NOT NULL DEFAULT 'Pending',
    FilledDate DATETIME,
    FilledBy VARCHAR(100),
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE,
    INDEX IX_Prescriptions_PatientId (PatientId),
    INDEX IX_Prescriptions_DoctorId (DoctorId),
    INDEX IX_Prescriptions_PrescriptionDate (PrescriptionDate),
    INDEX IX_Prescriptions_Status (Status),
    INDEX IX_Prescriptions_IsDeleted (IsDeleted)
);

-- Create PatientMedicines table
CREATE TABLE IF NOT EXISTS PatientMedicines (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PrescriptionId INT NOT NULL,
    MedicineId INT NOT NULL,
    Quantity INT NOT NULL,
    Instructions VARCHAR(200),
    Dosage VARCHAR(100),
    Frequency VARCHAR(50),
    Duration VARCHAR(50),
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE,
    FOREIGN KEY (PrescriptionId) REFERENCES Prescriptions(Id) ON DELETE CASCADE,
    FOREIGN KEY (MedicineId) REFERENCES Medicines(Id) ON DELETE RESTRICT,
    INDEX IX_PatientMedicines_PrescriptionId (PrescriptionId),
    INDEX IX_PatientMedicines_MedicineId (MedicineId),
    INDEX IX_PatientMedicines_IsDeleted (IsDeleted)
);

-- Insert sample medicines
INSERT INTO Medicines (Name, Description, DiseaseType, Price, Quantity, ExpiryDate, Manufacturer, DosageForm, DosageStrength, CreatedAt, UpdatedAt, IsDeleted) VALUES
('Paracetamol', 'Pain reliever and fever reducer', 'Pain Management', 5.50, 100, '2025-12-31 23:59:59', 'Generic Pharma', 'Tablet', '500mg', NOW(), NOW(), FALSE),
('Amoxicillin', 'Antibiotic for bacterial infections', 'Infections', 12.75, 50, '2025-06-30 23:59:59', 'MedCorp', 'Capsule', '250mg', NOW(), NOW(), FALSE),
('Insulin', 'Diabetes management medication', 'Diabetes', 45.00, 25, '2025-03-31 23:59:59', 'DiabeticCare', 'Injection', '100 units/ml', NOW(), NOW(), FALSE),
('Lisinopril', 'Blood pressure medication', 'Cardiovascular', 8.25, 75, '2025-09-30 23:59:59', 'HeartMed', 'Tablet', '10mg', NOW(), NOW(), FALSE),
('Metformin', 'Type 2 diabetes medication', 'Diabetes', 6.50, 80, '2025-08-31 23:59:59', 'DiabeticCare', 'Tablet', '500mg', NOW(), NOW(), FALSE),
('Aspirin', 'Anti-inflammatory and blood thinner', 'Cardiovascular', 3.25, 120, '2025-11-30 23:59:59', 'Generic Pharma', 'Tablet', '81mg', NOW(), NOW(), FALSE),
('Ibuprofen', 'Anti-inflammatory pain reliever', 'Pain Management', 4.75, 90, '2025-07-31 23:59:59', 'PainRelief Inc', 'Tablet', '200mg', NOW(), NOW(), FALSE),
('Omeprazole', 'Acid reflux medication', 'Digestive', 9.50, 60, '2025-10-31 23:59:59', 'Digestive Health', 'Capsule', '20mg', NOW(), NOW(), FALSE);

-- Insert sample prescriptions
INSERT INTO Prescriptions (PatientId, DoctorId, PrescriptionDate, Notes, Status, CreatedAt, UpdatedAt, IsDeleted) VALUES
(1, 1, NOW(), 'Patient has high blood pressure, monitor regularly', 'Pending', NOW(), NOW(), FALSE),
(2, 2, NOW(), 'Diabetic patient, check blood sugar levels', 'Filled', NOW(), NOW(), FALSE),
(3, 1, NOW(), 'Post-surgery pain management', 'Pending', NOW(), NOW(), FALSE);

-- Insert sample patient medicines
INSERT INTO PatientMedicines (PrescriptionId, MedicineId, Quantity, Instructions, Dosage, Frequency, Duration, CreatedAt, UpdatedAt, IsDeleted) VALUES
(1, 4, 30, 'Take with food', '1 tablet', 'Once daily', '30 days', NOW(), NOW(), FALSE),
(2, 3, 1, 'Inject as directed', '1 vial', 'As needed', '30 days', NOW(), NOW(), FALSE),
(2, 5, 60, 'Take with meals', '1 tablet', 'Twice daily', '30 days', NOW(), NOW(), FALSE),
(3, 1, 20, 'Take as needed for pain', '1-2 tablets', 'Every 6 hours', '7 days', NOW(), NOW(), FALSE);

COMMIT;


