# Hospital Management System (HMS) - Milestone 2 Documentation
## Appointment & Scheduling System

**Version:** 2.0  
**Date:** December 2024  
**Previous Milestone:** Patient & Doctor Management (Milestone 1) ✅

---

## Executive Summary

This document outlines the implementation plan for Milestone 2 of the Hospital Management System, focusing on the **Appointment & Scheduling System**. Building upon the existing Patient and Doctor management modules from Milestone 1, this milestone introduces appointment booking, scheduling, and management capabilities with JWT authentication and comprehensive logging.

---

## 1. Current System State (Milestone 1 Complete)

### ✅ Already Implemented:
- **Patient Management**: Full CRUD operations with soft delete
- **Doctor Management**: Full CRUD operations with specialization tracking
- **Database**: MySQL with Entity Framework Core 8.0
- **Architecture**: Repository pattern with AutoMapper and FluentValidation
- **API Documentation**: Swagger/OpenAPI integration
- **Database Seeding**: Initial data population

### 📁 Current Project Structure:
```
HospitalManagementSystem/
├── Controllers/
│   ├── PatientsController.cs ✅
│   └── DoctorsController.cs ✅
├── Models/
│   ├── Entities/
│   │   ├── Patient.cs ✅
│   │   └── Doctor.cs ✅
│   └── DTOs/
│       ├── PatientDto.cs ✅
│       └── DoctorDto.cs ✅
├── Services/
│   ├── PatientService.cs ✅
│   ├── DoctorService.cs ✅
│   ├── IPatientService.cs ✅
│   └── IDoctorService.cs ✅
├── Data/
│   ├── HospitalDbContext.cs ✅
│   └── DbSeeder.cs ✅
└── Repositories/
    ├── IRepository.cs ✅
    └── Repository.cs ✅
```

---

## 2. Milestone 2 Objectives

### Primary Goals:
1. **Appointment Management System**: Enable patients to book appointments with doctors
2. **JWT Authentication**: Implement secure API access with role-based authorization
3. **Advanced Logging**: Integrate Serilog for comprehensive application monitoring
4. **Data Relationships**: Establish Patient ↔ Appointment ↔ Doctor relationships
5. **Business Logic**: Implement appointment validation and conflict prevention

### Success Criteria:
- ✅ Patients can book appointments with available doctors
- ✅ Doctors can view their appointment schedules
- ✅ System prevents double-booking and validates doctor availability
- ✅ JWT authentication protects all endpoints
- ✅ Comprehensive logging tracks all operations
- ✅ API supports filtering by doctor, patient, and date

---

## 3. Technical Architecture for Milestone 2

### 3.1 New Components to Add:

```
HospitalManagementSystem/
├── Controllers/
│   └── AppointmentsController.cs 🆕
├── Models/
│   ├── Entities/
│   │   ├── Appointment.cs 🆕
│   │   └── User.cs 🆕
│   ├── DTOs/
│   │   ├── AppointmentDto.cs 🆕
│   │   ├── CreateAppointmentDto.cs 🆕
│   │   ├── UpdateAppointmentDto.cs 🆕
│   │   └── LoginDto.cs 🆕
│   └── ViewModels/
│       └── AppointmentViewModel.cs 🆕
├── Services/
│   ├── AppointmentService.cs 🆕
│   ├── IAppointmentService.cs 🆕
│   ├── AuthService.cs 🆕
│   └── IAuthService.cs 🆕
├── Middleware/
│   └── JwtMiddleware.cs 🆕
├── Validators/
│   ├── AppointmentDtoValidator.cs 🆕
│   └── LoginDtoValidator.cs 🆕
└── Extensions/
    └── ServiceExtensions.cs 🆕
```

---

## 4. Database Schema Updates

### 4.1 New Tables:

#### **Appointments Table**
```sql
CREATE TABLE Appointments (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    PatientId INT NOT NULL,
    DoctorId INT NOT NULL,
    AppointmentDate DATETIME NOT NULL,
    Duration INT NOT NULL DEFAULT 30, -- in minutes
    Status ENUM('Scheduled', 'Completed', 'Cancelled', 'NoShow') DEFAULT 'Scheduled',
    Notes TEXT NULL,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted BOOLEAN DEFAULT FALSE,
    
    FOREIGN KEY (PatientId) REFERENCES Patients(Id),
    FOREIGN KEY (DoctorId) REFERENCES Doctors(Id),
    
    INDEX IX_Appointments_DoctorId_AppointmentDate (DoctorId, AppointmentDate),
    INDEX IX_Appointments_PatientId (PatientId),
    INDEX IX_Appointments_Status (Status)
);
```

#### **Users Table (for Authentication)**
```sql
CREATE TABLE Users (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Username VARCHAR(50) UNIQUE NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    Role ENUM('Admin', 'Doctor', 'Patient', 'Pharmacist', 'Finance') NOT NULL,
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);
```

### 4.2 Updated Entity Relationships:

#### **Doctor Entity Updates**
```csharp
public class Doctor
{
    // ... existing properties ...
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public User? User { get; set; } // Navigation to User for authentication
}
```

#### **Patient Entity Updates**
```csharp
public class Patient
{
    // ... existing properties ...
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public User? User { get; set; } // Navigation to User for authentication
}
```

---

## 5. API Endpoints Specification

### 5.1 Authentication Endpoints
```
POST /api/auth/login
POST /api/auth/register
POST /api/auth/refresh-token
```

### 5.2 Appointment Management Endpoints
```
GET    /api/appointments                    # Get all appointments (with filtering)
GET    /api/appointments/{id}               # Get appointment by ID
POST   /api/appointments                    # Create new appointment
PUT    /api/appointments/{id}               # Update appointment
DELETE /api/appointments/{id}               # Cancel appointment
GET    /api/appointments/doctor/{doctorId}  # Get appointments by doctor
GET    /api/appointments/patient/{patientId} # Get appointments by patient
GET    /api/appointments/available-slots    # Get available time slots
```

### 5.3 Enhanced Patient/Doctor Endpoints
```
GET /api/patients/{id}/appointments         # Get patient's appointments
GET /api/doctors/{id}/appointments          # Get doctor's appointments
GET /api/doctors/{id}/schedule              # Get doctor's schedule
```

---

## 6. Implementation Steps (Visual Studio Workflow)

### Phase 1: Database & Entity Setup (Steps 1-3)

#### **Step 1: Install Required NuGet Packages**
```bash
# In Package Manager Console or via NuGet Package Manager UI:
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
Install-Package System.IdentityModel.Tokens.Jwt
Install-Package BCrypt.Net-Next
Install-Package Serilog.AspNetCore
Install-Package Serilog.Sinks.File
Install-Package Serilog.Sinks.Console
```

#### **Step 2: Create New Entity Models**
1. **Create `Models/Entities/Appointment.cs`**
2. **Create `Models/Entities/User.cs`**
3. **Update existing `Doctor.cs` and `Patient.cs`** with navigation properties

#### **Step 3: Update Database Context**
1. **Add new DbSets** to `HospitalDbContext.cs`
2. **Configure entity relationships** in `OnModelCreating`
3. **Create and run migration**:
   ```bash
   Add-Migration AddAppointmentsAndUsers
   Update-Database
   ```

### Phase 2: DTOs and Validation (Steps 4-6)

#### **Step 4: Create Appointment DTOs**
1. **Create `Models/DTOs/AppointmentDto.cs`**
2. **Create `Models/DTOs/CreateAppointmentDto.cs`**
3. **Create `Models/DTOs/UpdateAppointmentDto.cs`**
4. **Create `Models/DTOs/LoginDto.cs`**

#### **Step 5: Create Validators**
1. **Create `Validators/AppointmentDtoValidator.cs`**
2. **Create `Validators/LoginDtoValidator.cs`**

#### **Step 6: Update AutoMapper Configuration**
1. **Update `Mappings/MappingProfile.cs`** with new mappings

### Phase 3: Services Layer (Steps 7-9)

#### **Step 7: Create Authentication Service**
1. **Create `Services/IAuthService.cs`**
2. **Create `Services/AuthService.cs`**
3. **Implement JWT token generation and validation**

#### **Step 8: Create Appointment Service**
1. **Create `Services/IAppointmentService.cs`**
2. **Create `Services/AppointmentService.cs`**
3. **Implement appointment business logic and validation**

#### **Step 9: Create JWT Middleware**
1. **Create `Middleware/JwtMiddleware.cs`**
2. **Implement token validation and user context**

### Phase 4: Controllers and API (Steps 10-12)

#### **Step 10: Create Appointments Controller**
1. **Create `Controllers/AppointmentsController.cs`**
2. **Implement all appointment endpoints**
3. **Add proper authorization attributes**

#### **Step 11: Create Authentication Controller**
1. **Create `Controllers/AuthController.cs`**
2. **Implement login and registration endpoints**

#### **Step 12: Update Existing Controllers**
1. **Add authorization attributes** to existing controllers
2. **Add appointment-related endpoints** to Patient and Doctor controllers

### Phase 5: Configuration and Logging (Steps 13-15)

#### **Step 13: Configure JWT Authentication**
1. **Update `Program.cs`** with JWT configuration
2. **Add authentication middleware**
3. **Configure authorization policies**

#### **Step 14: Configure Serilog Logging**
1. **Update `Program.cs`** with Serilog configuration
2. **Add structured logging throughout the application**
3. **Configure log levels and output destinations**

#### **Step 15: Update Service Registration**
1. **Register new services** in `Program.cs`
2. **Configure dependency injection**
3. **Add middleware pipeline**

### Phase 6: Testing and Validation (Steps 16-18)

#### **Step 16: Update Database Seeder**
1. **Add sample users** to `DbSeeder.cs`
2. **Add sample appointments** for testing
3. **Create test data for different roles**

#### **Step 17: API Testing**
1. **Test all endpoints** using Swagger UI
2. **Verify JWT authentication** works correctly
3. **Test appointment validation** and conflict prevention

#### **Step 18: Documentation Update**
1. **Update Swagger documentation**
2. **Add XML comments** for new endpoints
3. **Test API documentation** in Swagger UI

---

## 7. Key Features Implementation

### 7.1 Appointment Validation Logic
- **Time Slot Validation**: Ensure appointments don't overlap
- **Doctor Availability**: Check if doctor is available at requested time
- **Business Hours**: Validate appointment times are within hospital hours
- **Future Dates Only**: Prevent booking appointments in the past

### 7.2 JWT Authentication Flow
1. **User Login**: Validate credentials and generate JWT token
2. **Token Validation**: Middleware validates tokens on protected endpoints
3. **Role-Based Access**: Different permissions for different user roles
4. **Token Refresh**: Implement token refresh mechanism

### 7.3 Logging Strategy
- **Request/Response Logging**: Log all API calls
- **Authentication Events**: Log login attempts and token generation
- **Business Logic Events**: Log appointment creation, updates, cancellations
- **Error Logging**: Comprehensive error tracking and debugging

---

## 8. Security Considerations

### 8.1 Authentication Security
- **Password Hashing**: Use BCrypt for secure password storage
- **JWT Security**: Implement proper JWT token validation
- **Role-Based Authorization**: Restrict access based on user roles
- **Token Expiration**: Implement reasonable token lifetimes

### 8.2 Data Protection
- **Input Validation**: Comprehensive validation on all inputs
- **SQL Injection Prevention**: Use parameterized queries
- **Sensitive Data**: Encrypt sensitive patient information
- **Audit Trail**: Log all data modifications

---

## 9. Testing Strategy

### 9.1 Unit Testing
- **Service Layer Testing**: Test appointment business logic
- **Validation Testing**: Test all validation rules
- **Authentication Testing**: Test JWT token generation and validation

### 9.2 Integration Testing
- **API Endpoint Testing**: Test all endpoints with different scenarios
- **Database Testing**: Test data persistence and relationships
- **Authentication Flow Testing**: Test complete authentication workflows

### 9.3 Manual Testing Scenarios
1. **Patient books appointment** with available doctor
2. **Doctor views schedule** and upcoming appointments
3. **Admin manages appointments** and user accounts
4. **System prevents double-booking** scenarios
5. **Authentication works** across all protected endpoints

---

## 10. Deployment Considerations

### 10.1 Configuration Updates
- **JWT Secret Key**: Configure secure secret key
- **Database Connection**: Ensure new tables are created
- **Logging Configuration**: Set up log file locations
- **CORS Settings**: Configure for frontend integration

### 10.2 Environment Variables
```json
{
  "JWT": {
    "SecretKey": "your-super-secret-key-here",
    "Issuer": "HospitalManagementSystem",
    "Audience": "HospitalManagementSystem",
    "ExpiryMinutes": 60
  },
  "Logging": {
    "LogLevel": "Information",
    "FilePath": "logs/hospital-management-{Date}.log"
  }
}
```

---

## 11. Success Metrics

### 11.1 Functional Metrics
- ✅ All appointment CRUD operations work correctly
- ✅ JWT authentication protects all endpoints
- ✅ Appointment validation prevents conflicts
- ✅ Role-based access control functions properly
- ✅ Comprehensive logging captures all events

### 11.2 Performance Metrics
- ✅ API response times under 200ms for simple operations
- ✅ Database queries optimized with proper indexing
- ✅ JWT token validation adds minimal overhead
- ✅ Logging doesn't impact application performance

### 11.3 Security Metrics
- ✅ All endpoints require proper authentication
- ✅ Sensitive data is properly protected
- ✅ Input validation prevents malicious data
- ✅ Audit trail captures all important events

---

## 12. Next Steps (Milestone 3 Preview)

After completing Milestone 2, the system will be ready for:
- **Medical Records Management**
- **Pharmacy Integration**
- **Billing and Finance Module**
- **Advanced Reporting and Analytics**
- **Real-time Notifications**

---

## 13. Appendix: Sample API Requests

### 13.1 Authentication
```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "doctor1",
  "password": "password123"
}
```

### 13.2 Create Appointment
```http
POST /api/appointments
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "patientId": 1,
  "doctorId": 2,
  "appointmentDate": "2024-12-20T10:00:00Z",
  "duration": 30,
  "notes": "Regular checkup"
}
```

### 13.3 Get Doctor's Appointments
```http
GET /api/appointments/doctor/2?date=2024-12-20
Authorization: Bearer <jwt-token>
```

---

**Document Version:** 2.0  
**Last Updated:** December 2024  
**Next Review:** Upon Milestone 2 Completion
