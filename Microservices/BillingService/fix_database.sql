-- Create database if it doesn't exist
CREATE DATABASE IF NOT EXISTS HospitalBilling;

-- Use the database
USE HospitalBilling;

-- Create Transactions table
CREATE TABLE IF NOT EXISTS Transactions (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PatientId INT NOT NULL,
    TransactionType VARCHAR(50) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Description VARCHAR(200),
    PaymentMethod VARCHAR(50),
    ReferenceNumber VARCHAR(100),
    TransactionDate DATETIME NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE,
    INDEX idx_patient_id (PatientId),
    INDEX idx_transaction_date (TransactionDate)
);

-- Create Expenditures table
CREATE TABLE IF NOT EXISTS Expenditures (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Category VARCHAR(100) NOT NULL,
    Description VARCHAR(200) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Vendor VARCHAR(100),
    ReferenceNumber VARCHAR(100),
    ExpenditureDate DATETIME NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE,
    INDEX idx_category (Category),
    INDEX idx_expenditure_date (ExpenditureDate)
);

-- Create Invoices table
CREATE TABLE IF NOT EXISTS Invoices (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PatientId INT NOT NULL,
    InvoiceNumber VARCHAR(50) NOT NULL UNIQUE,
    TotalAmount DECIMAL(18,2) NOT NULL,
    PaidAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    BalanceAmount DECIMAL(18,2) NOT NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending',
    Description VARCHAR(500),
    InvoiceDate DATETIME NOT NULL,
    DueDate DATETIME,
    PaidDate DATETIME,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE,
    INDEX idx_patient_id (PatientId),
    INDEX idx_invoice_number (InvoiceNumber),
    INDEX idx_status (Status),
    INDEX idx_invoice_date (InvoiceDate)
);
