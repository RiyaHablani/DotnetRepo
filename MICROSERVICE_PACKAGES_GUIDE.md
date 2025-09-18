# 🏥 Hospital Management System - Microservice-wise Package Guide

## 📦 Package Requirements by Microservice

### **🔐 AuthService** (Port: 5001/7001)
**Purpose**: Authentication & Authorization

#### Required NuGet Packages:
```xml
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="Serilog" Version="4.3.0" />
<PackageReference Include="Serilog.AspNetCore" Version="9.0.0" />
<PackageReference Include="Serilog.Sinks.File" Version="7.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
<PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
<PackageReference Include="FluentValidation" Version="11.8.0" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.14.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
```

#### Package Categories:
- **Authentication**: JWT Bearer, BCrypt
- **Database**: Entity Framework, MySQL
- **Logging**: Serilog with file output
- **API**: Swagger documentation
- **Validation**: FluentValidation
- **Mapping**: AutoMapper

---

### **👨‍⚕️ DoctorService** (Port: 5002/7002)
**Purpose**: Doctor Management

#### Required NuGet Packages:
```xml
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="Serilog" Version="4.3.0" />
<PackageReference Include="Serilog.AspNetCore" Version="9.0.0" />
<PackageReference Include="Serilog.Sinks.File" Version="7.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
<PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
<PackageReference Include="FluentValidation" Version="11.8.0" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.14.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
```

#### Package Categories:
- **Authentication**: JWT Bearer, BCrypt
- **Database**: Entity Framework, MySQL
- **Logging**: Serilog with file output
- **API**: Swagger documentation
- **Validation**: FluentValidation
- **Mapping**: AutoMapper

---

### **🏥 PatientService** (Port: 5003/7003)
**Purpose**: Patient Management

#### Required NuGet Packages:
```xml
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="Serilog" Version="4.3.0" />
<PackageReference Include="Serilog.AspNetCore" Version="9.0.0" />
<PackageReference Include="Serilog.Sinks.File" Version="7.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
<PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
<PackageReference Include="FluentValidation" Version="11.8.0" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.14.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
```

#### Package Categories:
- **Authentication**: JWT Bearer, BCrypt
- **Database**: Entity Framework, MySQL
- **Logging**: Serilog with file output
- **API**: Swagger documentation
- **Validation**: FluentValidation
- **Mapping**: AutoMapper

---

### **📅 AppointmentService** (Port: 5004/7004)
**Purpose**: Appointment Scheduling

#### Required NuGet Packages:
```xml
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="Serilog" Version="4.3.0" />
<PackageReference Include="Serilog.AspNetCore" Version="9.0.0" />
<PackageReference Include="Serilog.Sinks.File" Version="7.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
<PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
<PackageReference Include="FluentValidation" Version="11.8.0" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.14.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
```

#### Package Categories:
- **Authentication**: JWT Bearer, BCrypt
- **Database**: Entity Framework, MySQL
- **Logging**: Serilog with file output
- **API**: Swagger documentation
- **Validation**: FluentValidation
- **Mapping**: AutoMapper

---

### **📋 MedicalRecordsService** (Port: 5005/7005)
**Purpose**: Medical Records Management

#### Required NuGet Packages:
```xml
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="Serilog" Version="4.3.0" />
<PackageReference Include="Serilog.AspNetCore" Version="9.0.0" />
<PackageReference Include="Serilog.Sinks.File" Version="7.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
<PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
<PackageReference Include="FluentValidation" Version="11.8.0" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.14.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
```

#### Package Categories:
- **Authentication**: JWT Bearer, BCrypt
- **Database**: Entity Framework, MySQL
- **Logging**: Serilog with file output
- **API**: Swagger documentation
- **Validation**: FluentValidation
- **Mapping**: AutoMapper

---

### **💊 PharmacyService** (Port: 5006/7006)
**Purpose**: Pharmacy & Prescription Management

#### Required NuGet Packages:
```xml
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="Serilog" Version="4.3.0" />
<PackageReference Include="Serilog.AspNetCore" Version="9.0.0" />
<PackageReference Include="Serilog.Sinks.File" Version="7.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
<PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
<PackageReference Include="FluentValidation" Version="11.8.0" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.14.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
```

#### Package Categories:
- **Authentication**: JWT Bearer, BCrypt
- **Database**: Entity Framework, MySQL
- **Logging**: Serilog with file output
- **API**: Swagger documentation
- **Validation**: FluentValidation
- **Mapping**: AutoMapper

---

### **💰 BillingService** (Port: 5000/7000)
**Purpose**: Billing & Transaction Management

#### Required NuGet Packages:
```xml
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="Serilog" Version="4.3.0" />
<PackageReference Include="Serilog.AspNetCore" Version="9.0.0" />
<PackageReference Include="Serilog.Sinks.File" Version="7.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
<PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
<PackageReference Include="FluentValidation" Version="11.8.0" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.14.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
```

#### Package Categories:
- **Authentication**: JWT Bearer, BCrypt
- **Database**: Entity Framework, MySQL
- **Logging**: Serilog with file output
- **API**: Swagger documentation
- **Validation**: FluentValidation
- **Mapping**: AutoMapper

---

## 📊 **Package Summary**

### **Common Packages Across All Services:**
All microservices use the **exact same package set**:

1. **BCrypt.Net-Next** (4.0.3) - Password hashing
2. **Serilog** (4.3.0) - Logging framework
3. **Serilog.AspNetCore** (9.0.0) - ASP.NET Core integration
4. **Serilog.Sinks.File** (7.0.0) - File logging
5. **Swashbuckle.AspNetCore** (6.6.2) - API documentation
6. **Pomelo.EntityFrameworkCore.MySql** (8.0.0) - MySQL provider
7. **Microsoft.EntityFrameworkCore.Tools** (8.0.0) - EF tools
8. **Microsoft.EntityFrameworkCore.Design** (8.0.0) - EF design-time
9. **AutoMapper** (12.0.1) - Object mapping
10. **AutoMapper.Extensions.Microsoft.DependencyInjection** (12.0.1) - DI integration
11. **FluentValidation** (11.8.0) - Validation framework
12. **FluentValidation.AspNetCore** (11.3.0) - ASP.NET Core integration
13. **System.IdentityModel.Tokens.Jwt** (8.14.0) - JWT tokens
14. **Microsoft.AspNetCore.Authentication.JwtBearer** (8.0.0) - JWT authentication

### **Total Package Count:**
- **14 NuGet packages** per microservice
- **7 microservices** total
- **98 total package references** across all services

## 🚀 **Installation Commands**

### **For Individual Service:**
```bash
# Navigate to specific service
cd Microservices\AuthService

# Restore packages
dotnet restore

# Build the service
dotnet build

# Run the service
dotnet run
```

### **For All Services:**
```bash
# From root directory
dotnet restore
dotnet build
```

## 📝 **Notes**

1. **All services use identical packages** - This is actually good for consistency
2. **Packages are automatically restored** when you build the solution
3. **No additional packages needed** for individual services
4. **Each service is self-contained** with its own dependencies
5. **Shared packages** ensure consistent behavior across services

## ✅ **Verification**

To verify packages are installed correctly:
```bash
# Check packages for specific service
cd Microservices\AuthService
dotnet list package

# Check all packages in solution
dotnet list package --include-transitive
```

This microservice-wise package guide shows that all your services use the same package set, which is actually a good architectural decision for consistency! 🚀

