# 🚀 Hospital Management System - How to Run Project

## 📋 Prerequisites Checklist

Before running the project, ensure you have:

- ✅ **Visual Studio 2022** (with ASP.NET workload)
- ✅ **.NET 8 SDK** installed
- ✅ **MySQL Server 8.0+** running
- ✅ **MySQL Workbench** (optional but recommended)
- ✅ **Git** (if cloning from repository)

## 🔧 Quick Setup (5 Minutes)

### Step 1: Database Setup
```sql
-- Open MySQL Workbench or command line
-- Run these commands:
DROP DATABASE IF EXISTS HospitalManagementDB;
CREATE DATABASE HospitalManagementDB;
USE HospitalManagementDB;
```

### Step 2: Fix Architecture Issues
```bash
# Navigate to project root
cd C:\Users\DELL\source\repos\HospitalManagementSystem\HospitalManagementSystem

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

### Step 3: Restore Packages
```bash
# Restore all packages
dotnet restore

# Build the solution
dotnet build
```

## 🚀 Running the Project

### **Method 1: Run Individual Services (Recommended)**

#### Option A: Using Visual Studio
1. **Open Visual Studio 2022**
2. **Open `HospitalManagementSystem.sln`**
3. **Right-click on a service project** (e.g., AuthService)
4. **Select "Set as Startup Project"**
5. **Press F5** or click "Start" button
6. **Repeat for other services** in separate Visual Studio instances

#### Option B: Using Command Line
```bash
# Terminal 1 - AuthService
cd Microservices\AuthService
dotnet run

# Terminal 2 - DoctorService
cd Microservices\DoctorService
dotnet run

# Terminal 3 - PatientService
cd Microservices\PatientService
dotnet run

# Terminal 4 - AppointmentService
cd Microservices\AppointmentService
dotnet run

# Terminal 5 - MedicalRecordsService
cd Microservices\MedicalRecordsService
dotnet run

# Terminal 6 - PharmacyService
cd Microservices\PharmacyService
dotnet run

# Terminal 7 - BillingService
cd Microservices\BillingService
dotnet run
```

### **Method 2: Run Multiple Services Simultaneously**

#### Using Visual Studio
1. **Right-click on solution** in Solution Explorer
2. **Select "Properties"**
3. **Set "Multiple startup projects"**
4. **Set each service to "Start"**
5. **Click "OK"**
6. **Press F5** to run all services

#### Using Command Line (PowerShell)
```powershell
# Create a script to run all services
# Save as "run-all-services.ps1"

Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd Microservices\AuthService; dotnet run"
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd Microservices\DoctorService; dotnet run"
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd Microservices\PatientService; dotnet run"
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd Microservices\AppointmentService; dotnet run"
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd Microservices\MedicalRecordsService; dotnet run"
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd Microservices\PharmacyService; dotnet run"
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd Microservices\BillingService; dotnet run"
```

## 🌐 Service URLs and Ports

After running the services, access them at:

| Service | HTTP URL | HTTPS URL | Swagger UI |
|---------|----------|-----------|------------|
| **AuthService** | http://localhost:5001 | https://localhost:7001 | https://localhost:7001/swagger |
| **DoctorService** | http://localhost:5002 | https://localhost:7002 | https://localhost:7002/swagger |
| **PatientService** | http://localhost:5003 | https://localhost:7003 | https://localhost:7003/swagger |
| **AppointmentService** | http://localhost:5004 | https://localhost:7004 | https://localhost:7004/swagger |
| **MedicalRecordsService** | http://localhost:5005 | https://localhost:7005 | https://localhost:7005/swagger |
| **PharmacyService** | http://localhost:5006 | https://localhost:7006 | https://localhost:7006/swagger |
| **BillingService** | http://localhost:5000 | https://localhost:7000 | https://localhost:7000/swagger |

## 🧪 Testing the Application

### Step 1: Test Authentication
1. **Open AuthService Swagger**: https://localhost:7001/swagger
2. **Test login endpoint** with these credentials:
   - **Admin**: `admin` / `admin123`
   - **Doctor**: `dr.anderson` / `doctor123`
   - **Patient**: `john.smith` / `patient123`

### Step 2: Test Other Services
1. **Use the JWT token** from AuthService
2. **Add Authorization header**: `Bearer <your-token>`
3. **Test endpoints** in other services

### Step 3: Run Test Scripts
```powershell
# Run authentication tests
cd Microservices\AuthService
.\test-authentication.ps1
```

## 🔧 Troubleshooting

### Issue 1: "Only one compilation unit can have top-level statements"
**Solution**: Run each service as a separate project, not all together

### Issue 2: Database Connection Errors
**Solution**: 
1. Ensure MySQL server is running
2. Check connection string in `appsettings.json`
3. Verify database `HospitalManagementDB` exists

### Issue 3: Port Already in Use
**Solution**:
1. Check if another instance is running
2. Kill the process: `netstat -ano | findstr :5001`
3. Change port in `launchSettings.json`

### Issue 4: Services Not Starting
**Solution**:
1. Check Output window for errors
2. Verify all packages are restored
3. Check database connection
4. Review logs in `logs/` directory

## 📝 Development Workflow

### Daily Development
1. **Start AuthService first** (handles authentication)
2. **Start other services** as needed
3. **Use Swagger UI** for testing
4. **Check logs** for debugging

### Making Changes
1. **Make code changes**
2. **Save files** (Ctrl+S)
3. **Visual Studio auto-rebuilds** and restarts service
4. **Test changes** using Swagger UI

### Debugging
1. **Set breakpoints** in your code
2. **Press F5** to start debugging
3. **Step through code** using F10/F11

## 🎯 Quick Start Commands

```bash
# 1. Setup database
mysql -u root -p -e "DROP DATABASE IF EXISTS HospitalManagementDB; CREATE DATABASE HospitalManagementDB;"

# 2. Create solution file
dotnet new sln --name HospitalManagementSystem

# 3. Add projects to solution
dotnet sln add Microservices\AuthService\AuthService.csproj
dotnet sln add Microservices\DoctorService\DoctorService.csproj
dotnet sln add Microservices\PatientService\PatientService.csproj
dotnet sln add Microservices\AppointmentService\AppointmentService.csproj
dotnet sln add Microservices\MedicalRecordsService\MedicalRecordsService.csproj
dotnet sln add Microservices\PharmacyService\PharmacyService.csproj
dotnet sln add Microservices\BillingService\BillingService.csproj

# 4. Restore and build
dotnet restore
dotnet build

# 5. Run AuthService
cd Microservices\AuthService
dotnet run
```

## ✅ Success Indicators

You'll know the project is running correctly when:

- ✅ **All services start** without errors
- ✅ **Swagger UI opens** for each service
- ✅ **Database connection** is successful
- ✅ **Authentication works** with test credentials
- ✅ **Services communicate** with each other
- ✅ **Logs are generated** in `logs/` directory

## 🆘 Getting Help

If you encounter issues:

1. **Check the Output window** in Visual Studio
2. **Review logs** in the `logs/` directory
3. **Verify all prerequisites** are installed
4. **Check database connection** settings
5. **Ensure MySQL server** is running

---

**Happy Coding! 🚀**

This guide will get your Hospital Management System up and running in no time!

