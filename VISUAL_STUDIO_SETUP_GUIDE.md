# 🏥 Hospital Management System - Visual Studio Setup Guide

## 📋 Prerequisites

Before setting up the project in Visual Studio, ensure you have the following installed:

### Required Software
1. **Visual Studio 2022** (Community, Professional, or Enterprise)
   - Download from: [Visual Studio Official Website](https://visualstudio.microsoft.com/downloads/)
   - Ensure you have the **ASP.NET and web development** workload installed

2. **.NET 8 SDK**
   - Download from: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
   - Version: .NET 8.0 or later
   - Note: Usually comes with Visual Studio 2022, but verify installation

3. **MySQL Server 8.0+**
   - Download from: [MySQL Official Website](https://dev.mysql.com/downloads/mysql/)
   - Install with default settings
   - Remember your root password (or leave empty as configured)

4. **MySQL Workbench** (Optional but recommended)
   - Download from: [MySQL Workbench](https://dev.mysql.com/downloads/workbench/)
   - For database management and running SQL scripts

5. **Git** (if cloning from repository)
   - Download from: [Git Official Website](https://git-scm.com/downloads)

### Package Verification Commands
After installation, verify everything is working by running these commands in Command Prompt or PowerShell:

```bash
# Check .NET version (should show 8.0.x or later)
dotnet --version

# Check MySQL connection
mysql --version

# Check Git (if installed)
git --version
```

### NuGet Packages (Automatically Restored)
The following packages are already configured in the project files and will be automatically restored when you build the solution:

#### Core ASP.NET Packages
- `Microsoft.AspNetCore.App` (implicit)
- `Microsoft.AspNetCore.Authentication.JwtBearer` (8.0.0)
- `System.IdentityModel.Tokens.Jwt` (8.14.0)

#### Database Packages
- `Pomelo.EntityFrameworkCore.MySql` (8.0.0)
- `Microsoft.EntityFrameworkCore.Tools` (8.0.0)
- `Microsoft.EntityFrameworkCore.Design` (8.0.0)

#### API Documentation
- `Swashbuckle.AspNetCore` (6.6.2)

#### Logging Packages
- `Serilog` (4.3.0)
- `Serilog.AspNetCore` (9.0.0)
- `Serilog.Sinks.File` (7.0.0)

#### Security Packages
- `BCrypt.Net-Next` (4.0.3)

#### Mapping & Validation
- `AutoMapper` (12.0.1)
- `AutoMapper.Extensions.Microsoft.DependencyInjection` (12.0.1)
- `FluentValidation` (11.8.0)
- `FluentValidation.AspNetCore` (11.3.0)

### Quick Setup Summary
**Minimum Required Packages:**
1. ✅ Visual Studio 2022 (with ASP.NET workload)
2. ✅ .NET 8 SDK
3. ✅ MySQL Server 8.0+

**Optional but Recommended:**
4. ✅ MySQL Workbench
5. ✅ Git

**Automatically Installed:**
- All NuGet packages will be restored when you build the solution

## 🚀 Step-by-Step Setup Instructions

### Step 1: Open the Solution in Visual Studio

1. **Launch Visual Studio 2022**
2. **Open the solution**:
   - Click `File` → `Open` → `Project/Solution`
   - Navigate to your project folder: `C:\Users\DELL\source\repos\HospitalManagementSystem\HospitalManagementSystem`
   - Select `HospitalManagementSystem.sln` (if it exists) or the root folder
   - Click `Open`

### Step 2: Database Setup

#### 2.1 Create MySQL Database
1. **Open MySQL Workbench** (or use command line)
2. **Connect to your MySQL server** (usually `localhost:3306`)
3. **Run the following SQL commands**:
   ```sql
   -- Create the database
   DROP DATABASE IF EXISTS HospitalManagementDB;
   CREATE DATABASE HospitalManagementDB;
   USE HospitalManagementDB;
   
   -- The tables will be created automatically by Entity Framework migrations
   ```

#### 2.2 Verify Database Connection
1. **Check connection strings** in each service's `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=HospitalManagementDB;Uid=root;Pwd=;"
   }
   ```
2. **Update the connection string** if your MySQL credentials are different

### Step 3: Restore NuGet Packages

1. **Right-click on the solution** in Solution Explorer
2. **Select "Restore NuGet Packages"**
3. **Wait for all packages to be restored** (this may take a few minutes)

### Step 4: Build the Solution

1. **Right-click on the solution** in Solution Explorer
2. **Select "Rebuild Solution"**
3. **Wait for the build to complete** (check Output window for any errors)

### Step 5: Configure Multiple Startup Projects

Since this is a microservices architecture, you need to configure Visual Studio to run multiple projects simultaneously:

#### 5.1 Set Multiple Startup Projects
1. **Right-click on the solution** in Solution Explorer
2. **Select "Properties"**
3. **In the Solution Properties window**:
   - Select "Multiple startup projects"
   - Set the following projects to "Start":
     - `AuthService`
     - `DoctorService`
     - `PatientService`
     - `AppointmentService`
     - `MedicalRecordsService`
     - `PharmacyService`
     - `BillingService`
4. **Click "OK"**

#### 5.2 Alternative: Run Projects Individually
If you prefer to run projects one at a time:
1. **Right-click on each project** in Solution Explorer
2. **Select "Set as Startup Project"**
3. **Press F5** or click the "Start" button

### Step 6: Running the Application

#### Option A: Run All Services Simultaneously
1. **Press F5** or click the "Start" button
2. **Visual Studio will start all configured services**
3. **Wait for all services to start** (check Output window for startup messages)
4. **Browser windows will open** for each service's Swagger UI

#### Option B: Run Services Individually
1. **Right-click on a specific project** (e.g., AuthService)
2. **Select "Set as Startup Project"**
3. **Press F5** to run that service
4. **Repeat for other services** as needed

### Step 7: Verify the Application is Running

#### 7.1 Check Service URLs
After starting the services, you should see the following URLs in your browser:

| Service | HTTP URL | HTTPS URL | Swagger UI |
|---------|----------|-----------|------------|
| AuthService | http://localhost:5001 | https://localhost:7001 | https://localhost:7001/swagger |
| DoctorService | http://localhost:5002 | https://localhost:7002 | https://localhost:7002/swagger |
| PatientService | http://localhost:5003 | https://localhost:7003 | https://localhost:7003/swagger |
| AppointmentService | http://localhost:5004 | https://localhost:7004 | https://localhost:7004/swagger |
| MedicalRecordsService | http://localhost:5005 | https://localhost:7005 | https://localhost:7005/swagger |
| PharmacyService | http://localhost:5006 | https://localhost:7006 | https://localhost:7006/swagger |
| BillingService | http://localhost:5000 | https://localhost:7000 | https://localhost:7000/swagger |

#### 7.2 Test Authentication
1. **Open the AuthService Swagger UI**: `https://localhost:7001/swagger`
2. **Test the login endpoint** with these credentials:
   - **Admin**: `admin` / `admin123`
   - **Doctor**: `dr.anderson` / `doctor123`
   - **Patient**: `john.smith` / `patient123`

## 🔧 Troubleshooting Common Issues

### Issue 1: Database Connection Errors
**Problem**: "Unable to connect to database"
**Solution**:
1. Ensure MySQL server is running
2. Check connection string in `appsettings.json`
3. Verify database `HospitalManagementDB` exists
4. Check MySQL credentials

### Issue 2: Port Already in Use
**Problem**: "Port is already in use"
**Solution**:
1. Check if another instance is running
2. Kill the process using the port: `netstat -ano | findstr :5001`
3. Change the port in `launchSettings.json`

### Issue 3: NuGet Package Restore Fails
**Problem**: Package restore errors
**Solution**:
1. Clear NuGet cache: `Tools` → `NuGet Package Manager` → `Package Manager Console`
2. Run: `dotnet nuget locals all --clear`
3. Restore packages again

### Issue 4: Build Errors
**Problem**: Compilation errors
**Solution**:
1. Clean solution: `Build` → `Clean Solution`
2. Rebuild solution: `Build` → `Rebuild Solution`
3. Check for missing references

### Issue 5: Services Not Starting
**Problem**: Services fail to start
**Solution**:
1. Check the Output window for error messages
2. Verify all dependencies are installed
3. Check database connection
4. Review logs in the `logs/` directory

### Issue 6: "Only one compilation unit can have top-level statements"
**Problem**: Multiple Program.cs files with top-level statements
**Solution**:
1. **Run each microservice as a separate project** (Recommended)
   - Right-click on each service project
   - Select "Set as Startup Project"
   - Press F5 to run that specific service
2. **Use multiple terminals** to run services simultaneously
3. **Configure multiple startup projects** in Solution Properties
4. **Each microservice should run independently** on different ports

## 📝 Development Workflow

### Running Individual Services
1. **Right-click on the service project** in Solution Explorer
2. **Select "Set as Startup Project"**
3. **Press F5** to run
4. **Use Swagger UI** to test endpoints

### Debugging
1. **Set breakpoints** in your code
2. **Press F5** to start debugging
3. **Step through code** using F10 (Step Over) or F11 (Step Into)

### Making Changes
1. **Make your code changes**
2. **Save the files** (Ctrl+S)
3. **Visual Studio will automatically rebuild** and restart the service
4. **Test your changes** using Swagger UI

## 🧪 Testing the Application

### Using Swagger UI
1. **Navigate to any service's Swagger UI** (e.g., `https://localhost:7001/swagger`)
2. **Expand the endpoints** you want to test
3. **Click "Try it out"**
4. **Enter required parameters**
5. **Click "Execute"**

### Using the Test Scripts
1. **Open PowerShell** as Administrator
2. **Navigate to a service directory** (e.g., `Microservices/AuthService`)
3. **Run the test script**: `.\test-authentication.ps1`

## 📚 Additional Resources

### Visual Studio Shortcuts
- **F5**: Start debugging
- **Ctrl+F5**: Start without debugging
- **Shift+F5**: Stop debugging
- **Ctrl+Shift+B**: Build solution
- **Ctrl+Shift+O**: Open file

### Useful Visual Studio Extensions
- **NuGet Package Manager**
- **Git for Visual Studio**
- **MySQL for Visual Studio** (if available)

### Project Structure
```
HospitalManagementSystem/
├── Controllers/           # Main API controllers
├── Microservices/        # Microservices
│   ├── AuthService/      # Authentication service
│   ├── DoctorService/    # Doctor management
│   ├── PatientService/   # Patient management
│   ├── AppointmentService/ # Appointment scheduling
│   ├── MedicalRecordsService/ # Medical records
│   ├── PharmacyService/  # Pharmacy management
│   └── BillingService/   # Billing system
└── Properties/           # Project properties
```

## 🆘 Getting Help

If you encounter issues not covered in this guide:

1. **Check the Output window** in Visual Studio for error messages
2. **Review the logs** in the `logs/` directory of each service
3. **Check the Swagger UI** for API documentation
4. **Verify all prerequisites** are installed correctly
5. **Ensure MySQL server** is running and accessible

---

**Happy Coding! 🚀**

This guide should help you get the Hospital Management System up and running in Visual Studio. The microservices architecture allows for independent development and testing of each service.
