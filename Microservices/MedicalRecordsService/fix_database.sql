-- Medical Records & Laboratory Database Schema
-- This script creates the necessary tables for Milestone 3

-- Create MedicalRecords table
CREATE TABLE IF NOT EXISTS MedicalRecords (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    PatientId INT NOT NULL,
    DoctorId INT NOT NULL,
    Diagnosis VARCHAR(200) NOT NULL,
    Symptoms TEXT NULL,
    Treatment TEXT NULL,
    Notes TEXT NULL,
    RecordDate DATETIME NOT NULL,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted BOOLEAN DEFAULT FALSE,
    
    INDEX IX_MedicalRecords_PatientId (PatientId),
    INDEX IX_MedicalRecords_DoctorId (DoctorId),
    INDEX IX_MedicalRecords_RecordDate (RecordDate)
);

-- Create LabReports table
CREATE TABLE IF NOT EXISTS LabReports (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    PatientId INT NOT NULL,
    DoctorId INT NOT NULL,
    TestName VARCHAR(100) NOT NULL,
    TestDescription TEXT NULL,
    Results TEXT NULL,
    Status VARCHAR(100) DEFAULT 'Pending',
    Notes TEXT NULL,
    TestDate DATETIME NOT NULL,
    CompletedDate DATETIME NULL,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted BOOLEAN DEFAULT FALSE,
    
    INDEX IX_LabReports_PatientId (PatientId),
    INDEX IX_LabReports_DoctorId (DoctorId),
    INDEX IX_LabReports_TestDate (TestDate),
    INDEX IX_LabReports_Status (Status)
);

-- Insert sample data for testing
INSERT INTO MedicalRecords (PatientId, DoctorId, Diagnosis, Symptoms, Treatment, Notes, RecordDate, CreatedAt, UpdatedAt, IsDeleted) VALUES
(1, 1, 'Hypertension', 'High blood pressure, headaches', 'Lisinopril 10mg daily, lifestyle modifications', 'Patient advised to reduce salt intake and exercise regularly', '2024-01-15 10:30:00', NOW(), NOW(), FALSE),
(1, 1, 'Diabetes Type 2', 'Increased thirst, frequent urination', 'Metformin 500mg twice daily, blood glucose monitoring', 'Patient education provided on diabetes management', '2024-01-20 14:15:00', NOW(), NOW(), FALSE),
(2, 2, 'Common Cold', 'Runny nose, cough, mild fever', 'Rest, fluids, over-the-counter cold medication', 'Symptoms are mild, no antibiotics needed', '2024-01-18 09:45:00', NOW(), NOW(), FALSE),
(3, 3, 'Sprained Ankle', 'Pain and swelling in left ankle', 'RICE therapy, pain medication as needed', 'Patient advised to avoid weight-bearing activities for 1 week', '2024-01-22 16:20:00', NOW(), NOW(), FALSE);

INSERT INTO LabReports (PatientId, DoctorId, TestName, TestDescription, Results, Status, Notes, TestDate, CompletedDate, CreatedAt, UpdatedAt, IsDeleted) VALUES
(1, 1, 'Blood Pressure', 'Routine blood pressure measurement', '140/90 mmHg', 'Completed', 'Elevated blood pressure noted', '2024-01-15 10:30:00', '2024-01-15 10:35:00', NOW(), NOW(), FALSE),
(1, 1, 'Blood Glucose', 'Fasting blood glucose test', '180 mg/dL', 'Completed', 'Elevated glucose levels indicating diabetes', '2024-01-20 08:00:00', '2024-01-20 08:30:00', NOW(), NOW(), FALSE),
(1, 1, 'HbA1c', 'Hemoglobin A1c test for diabetes monitoring', '8.2%', 'Completed', 'Poor glycemic control, medication adjustment needed', '2024-01-20 08:00:00', '2024-01-20 09:00:00', NOW(), NOW(), FALSE),
(2, 2, 'Complete Blood Count', 'Routine CBC to check for infection', 'Normal range', 'Completed', 'No signs of bacterial infection', '2024-01-18 09:45:00', '2024-01-18 10:15:00', NOW(), NOW(), FALSE),
(3, 3, 'X-Ray Ankle', 'X-ray of left ankle to rule out fracture', 'No fracture detected, soft tissue swelling only', 'Completed', 'Confirmed sprain, no bone injury', '2024-01-22 16:20:00', '2024-01-22 17:00:00', NOW(), NOW(), FALSE),
(1, 1, 'Lipid Panel', 'Cholesterol and triglyceride levels', 'Pending', 'Pending', 'Test ordered for cardiovascular risk assessment', '2024-01-25 09:00:00', NULL, NOW(), NOW(), FALSE);

-- Verify the tables were created successfully
SELECT 'MedicalRecords table created successfully' as Status;
SELECT 'LabReports table created successfully' as Status;
SELECT COUNT(*) as MedicalRecordsCount FROM MedicalRecords;
SELECT COUNT(*) as LabReportsCount FROM LabReports;
