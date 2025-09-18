# 🔄 Hospital Management System - Microservice Running Flow

## 📋 Overview

The Hospital Management System follows a **microservices architecture** where each service runs independently and communicates with others. Here's the complete flow:

## 🏗️ Architecture Flow

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   AuthService   │    │  DoctorService  │    │ PatientService  │
│   (Port: 5001)  │    │   (Port: 5002)  │    │   (Port: 5003)  │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         │                       │                       │
         ▼                       ▼                       ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│ AppointmentService│   │MedicalRecordsService│  │ PharmacyService │
│   (Port: 5004)  │    │   (Port: 5005)  │    │   (Port: 5006)  │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         │                       │                       │
         ▼                       ▼                       ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│ BillingService  │    │   MySQL DB      │    │   Logs Folder   │
│   (Port: 5000)  │    │ (Port: 3306)    │    │   (File Logs)   │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## 🚀 Step-by-Step Running Flow

### **Phase 1: Prerequisites Setup**

#### 1.1 Database Setup
```sql
-- Create database
DROP DATABASE IF EXISTS HospitalManagementDB;
CREATE DATABASE HospitalManagementDB;
USE HospitalManagementDB;
```

#### 1.2 Solution File Creation
```bash
# Create solution file
dotnet new sln --name HospitalManagementSystem

# Add all microservices to solution
dotnet sln add Microservices\AuthService\AuthService.csproj
dotnet sln add Microservices\DoctorService\DoctorService.csproj
dotnet sln add Microservices\PatientService\PatientService.csproj
dotnet sln add Microservices\AppointmentService\AppointmentService.csproj
dotnet sln add Microservices\MedicalRecordsService\MedicalRecordsService.csproj
dotnet sln add Microservices\PharmacyService\PharmacyService.csproj
dotnet sln add Microservices\BillingService\BillingService.csproj
```

#### 1.3 Package Restoration
```bash
# Restore all packages
dotnet restore

# Build solution
dotnet build
```

### **Phase 2: Service Startup Flow**

#### 2.1 **CRITICAL: Start AuthService First** 🔐
```bash
# Terminal 1 - AuthService (MUST START FIRST)
cd Microservices\AuthService
dotnet run
```
**Why First?** All other services depend on JWT tokens from AuthService.

**What Happens:**
- ✅ Database connection established
- ✅ Tables created (Users, Patients, Doctors, Appointments)
- ✅ Sample data seeded (admin, doctors, patients)
- ✅ JWT authentication configured
- ✅ Swagger UI available at https://localhost:7001/swagger

#### 2.2 **Start Core Services** 🏥
```bash
# Terminal 2 - DoctorService
cd Microservices\DoctorService
dotnet run

# Terminal 3 - PatientService  
cd Microservices\PatientService
dotnet run
```

**What Happens:**
- ✅ Doctor management endpoints available
- ✅ Patient management endpoints available
- ✅ JWT authentication validated
- ✅ Database connections established

#### 2.3 **Start Business Logic Services** 📅
```bash
# Terminal 4 - AppointmentService
cd Microservices\AppointmentService
dotnet run

# Terminal 5 - MedicalRecordsService
cd Microservices\MedicalRecordsService
dotnet run
```

**What Happens:**
- ✅ Appointment scheduling available
- ✅ Medical records management available
- ✅ Inter-service communication established

#### 2.4 **Start Support Services** 💊
```bash
# Terminal 6 - PharmacyService
cd Microservices\PharmacyService
dotnet run

# Terminal 7 - BillingService
cd Microservices\BillingService
dotnet run
```

**What Happens:**
- ✅ Prescription management available
- ✅ Billing and payment processing available
- ✅ Complete system functionality

## 🔄 Service Communication Flow

### **Authentication Flow**
```
1. Client Request → AuthService
2. AuthService → Validates credentials
3. AuthService → Returns JWT token
4. Client → Uses token for other services
```

### **Data Flow Example (Appointment Booking)**
```
1. Patient → AuthService (Login)
2. AuthService → Returns JWT token
3. Patient → AppointmentService (Book appointment)
4. AppointmentService → Validates JWT token
5. AppointmentService → DoctorService (Check doctor availability)
6. AppointmentService → PatientService (Validate patient)
7. AppointmentService → Saves appointment to database
8. AppointmentService → Returns confirmation
```

### **Inter-Service Communication**
```
AuthService ←→ All Services (JWT validation)
DoctorService ←→ AppointmentService (Doctor availability)
PatientService ←→ AppointmentService (Patient validation)
AppointmentService ←→ MedicalRecordsService (Record creation)
PharmacyService ←→ PatientService (Prescription validation)
BillingService ←→ All Services (Payment processing)
```

## 📊 Service Dependencies

### **Dependency Matrix**
| Service | Depends On | Provides To |
|---------|------------|-------------|
| **AuthService** | MySQL DB | All Services (JWT) |
| **DoctorService** | MySQL DB, AuthService | AppointmentService, BillingService |
| **PatientService** | MySQL DB, AuthService | AppointmentService, PharmacyService, BillingService |
| **AppointmentService** | MySQL DB, AuthService, DoctorService, PatientService | MedicalRecordsService, BillingService |
| **MedicalRecordsService** | MySQL DB, AuthService | AppointmentService, PharmacyService |
| **PharmacyService** | MySQL DB, AuthService, PatientService | BillingService |
| **BillingService** | MySQL DB, AuthService | All Services (Payment) |

## 🎯 Running Scenarios

### **Scenario 1: Development Mode (All Services)**
```bash
# Start all services simultaneously
# Use multiple terminals or Visual Studio multiple startup projects
```

### **Scenario 2: Testing Mode (Individual Services)**
```bash
# Start only the service you're working on
# Other services can be mocked or stubbed
```

### **Scenario 3: Production Mode (Docker/Cloud)**
```bash
# Each service runs in its own container
# Load balancer distributes requests
# Service discovery handles communication
```

## 🔍 Service Health Checks

### **Startup Sequence Validation**
1. **AuthService** - Check JWT endpoint
2. **DoctorService** - Check doctor list endpoint
3. **PatientService** - Check patient list endpoint
4. **AppointmentService** - Check appointment list endpoint
5. **MedicalRecordsService** - Check records endpoint
6. **PharmacyService** - Check medicine list endpoint
7. **BillingService** - Check billing endpoint

### **Health Check URLs**
```
GET https://localhost:7001/api/auth/test
GET https://localhost:7002/api/doctors
GET https://localhost:7003/api/patients
GET https://localhost:7004/api/appointments
GET https://localhost:7005/api/medicalrecords
GET https://localhost:7006/api/medicines
GET https://localhost:7000/api/billing
```

## 🚨 Common Issues & Solutions

### **Issue 1: AuthService Not Starting**
**Problem**: Database connection failed
**Solution**: Check MySQL server, connection string

### **Issue 2: Other Services Failing**
**Problem**: JWT validation failing
**Solution**: Start AuthService first, check JWT configuration

### **Issue 3: Database Not Seeding**
**Problem**: Data not appearing in database
**Solution**: Check DbSeeder execution, database permissions

### **Issue 4: Inter-Service Communication Failing**
**Problem**: Services can't communicate
**Solution**: Check network connectivity, service URLs

## 📝 Best Practices

### **1. Always Start AuthService First**
- Required for JWT token generation
- Other services depend on it

### **2. Use Proper Startup Order**
- AuthService → Core Services → Business Logic → Support Services

### **3. Monitor Service Health**
- Check logs regularly
- Use health check endpoints
- Monitor database connections

### **4. Handle Failures Gracefully**
- Implement retry logic
- Use circuit breakers
- Log errors properly

## 🎯 Quick Start Commands

```bash
# 1. Setup database
mysql -u root -p -e "DROP DATABASE IF EXISTS HospitalManagementDB; CREATE DATABASE HospitalManagementDB;"

# 2. Create solution
dotnet new sln --name HospitalManagementSystem
dotnet sln add Microservices\AuthService\AuthService.csproj
# ... add other services

# 3. Restore and build
dotnet restore && dotnet build

# 4. Start services (in order)
cd Microservices\AuthService && dotnet run &
cd Microservices\DoctorService && dotnet run &
cd Microservices\PatientService && dotnet run &
cd Microservices\AppointmentService && dotnet run &
cd Microservices\MedicalRecordsService && dotnet run &
cd Microservices\PharmacyService && dotnet run &
cd Microservices\BillingService && dotnet run &
```

## ✅ Success Indicators

You'll know the system is running correctly when:

- ✅ **All 7 services start** without errors
- ✅ **Swagger UI opens** for each service
- ✅ **Database tables created** and seeded
- ✅ **JWT authentication works** with test credentials
- ✅ **Inter-service communication** established
- ✅ **Logs generated** in each service's logs folder
- ✅ **Health check endpoints** return 200 OK

---

**This flow ensures your Hospital Management System runs smoothly with proper service dependencies and communication! 🚀**

