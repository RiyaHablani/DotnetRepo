# Hospital Management System - Milestone 1 Planning
## Project Setup & Core Entities

### Overview
This milestone focuses on setting up the foundation of the Hospital Management System using .NET Core Web API with Entity Framework Core, specifically designed for Visual Studio development environment.

---

## Prerequisites & Visual Studio Setup

### Required Software
1. **Visual Studio 2022** (Community/Professional/Enterprise)
2. **SQL Server** (LocalDB or Express Edition)
3. **Postman** (for API testing)
4. **Git** (for version control)

### Visual Studio Extensions to Install
1. **Entity Framework Core Tools** (if not already installed)
2. **Swagger/OpenAPI** (usually included in Web API template)
3. **NuGet Package Manager** (built-in)

---

## Step-by-Step Implementation Plan

### Step 1: Create New Project in Visual Studio
1. Open Visual Studio 2022
2. Click "Create a new project"
3. Select "ASP.NET Core Web API" template
4. Project name: `HospitalManagementSystem`
5. Solution name: `HospitalManagementSystem`
6. Framework: `.NET 8.0`
7. Authentication: `None` (we'll implement JWT later)
8. Configure for HTTPS: `Yes`
9. Enable OpenAPI support: `Yes`
10. Click "Create"

### Step 2: Install Required NuGet Packages
Open Package Manager Console in Visual Studio and run these commands:

```powershell
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.EntityFrameworkCore.Design
Install-Package AutoMapper
Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection
Install-Package FluentValidation
Install-Package FluentValidation.AspNetCore
Install-Package Serilog
Install-Package Serilog.AspNetCore
Install-Package Serilog.Sinks.File
Install-Package Serilog.Sinks.Console
Install-Package Swashbuckle.AspNetCore
```

### Step 3: Project Structure Setup
Create the following folder structure in Solution Explorer:

```
HospitalManagementSystem/
├── Controllers/
├── Models/
│   ├── Entities/
│   └── DTOs/
├── Data/
├── Services/
├── Repositories/
├── Validators/
├── Mappings/
└── Middleware/
```

### Step 4: Database Configuration
1. **Create appsettings.json entries:**
   - Add connection string for SQL Server LocalDB
   - Configure logging settings

2. **Create DbContext:**
   - Create `HospitalDbContext.cs` in Data folder
   - Configure connection string
   - Set up DbSets for entities

### Step 5: Entity Models Creation
Create the following entity models in `Models/Entities/`:

1. **Patient.cs**
   - Properties: Id, Name, Age, Gender, Address, PhoneNumber, Email, DateOfBirth, CreatedDate, IsActive

2. **Doctor.cs**
   - Properties: Id, Name, Specialization, PhoneNumber, Email, LicenseNumber, CreatedDate, IsActive

3. **BaseEntity.cs** (for common properties)
   - Properties: Id, CreatedDate, ModifiedDate, IsActive

### Step 6: DTOs Creation
Create DTOs in `Models/DTOs/`:

1. **PatientDto.cs** (for API responses)
2. **CreatePatientDto.cs** (for API requests)
3. **DoctorDto.cs** (for API responses)
4. **CreateDoctorDto.cs** (for API requests)

### Step 7: Repository Pattern Implementation
1. **Create IRepository<T> interface**
2. **Create Repository<T> base class**
3. **Create IPatientRepository and PatientRepository**
4. **Create IDoctorRepository and DoctorRepository**

### Step 8: Service Layer Implementation
1. **Create IPatientService and PatientService**
2. **Create IDoctorService and DoctorService**
3. **Implement business logic for CRUD operations**

### Step 9: Controller Implementation
1. **Create PatientsController**
   - GET /api/patients (get all patients)
   - GET /api/patients/{id} (get patient by id)
   - POST /api/patients (create new patient)
   - PUT /api/patients/{id} (update patient)
   - DELETE /api/patients/{id} (soft delete patient)

2. **Create DoctorsController**
   - Similar CRUD operations for doctors

### Step 10: AutoMapper Configuration
1. **Create MappingProfile.cs**
2. **Configure entity to DTO mappings**
3. **Register AutoMapper in Program.cs**

### Step 11: Validation Setup
1. **Create PatientValidator.cs**
2. **Create DoctorValidator.cs**
3. **Configure FluentValidation in Program.cs**

### Step 12: Database Migration and Seeding
1. **Create initial migration**
2. **Create DbSeeder.cs for sample data**
3. **Seed 5+ patients and 5+ doctors**

### Step 13: Logging Configuration
1. **Configure Serilog in Program.cs**
2. **Set up file and console logging**
3. **Add logging to controllers and services**

### Step 14: Swagger Documentation
1. **Configure Swagger in Program.cs**
2. **Add XML comments for API documentation**
3. **Test Swagger UI**

### Step 15: Testing Setup
1. **Create test project** (HospitalManagementSystem.Tests)
2. **Install xUnit and Moq packages**
3. **Create unit tests for services**
4. **Create integration tests for controllers**

---

## Visual Studio Specific Tips

### Database Connection Setup
1. Use **SQL Server Object Explorer** in Visual Studio
2. Connect to LocalDB instance
3. Use **Server Explorer** to verify database creation

### Package Management
1. Use **Package Manager Console** for NuGet commands
2. Use **Manage NuGet Packages** UI for browsing packages
3. Always check package compatibility with .NET 8.0

### Code Organization
1. Use **Solution Explorer** to organize files
2. Use **Class View** to navigate between classes
3. Use **Object Browser** to explore dependencies

### Debugging
1. Set breakpoints in controllers and services
2. Use **Debug** menu to start debugging
3. Use **Output** window to view logs

---

## File Creation Order

1. **Models/Entities/BaseEntity.cs**
2. **Models/Entities/Patient.cs**
3. **Models/Entities/Doctor.cs**
4. **Data/HospitalDbContext.cs**
5. **Models/DTOs/** (all DTO files)
6. **Repositories/** (repository interfaces and implementations)
7. **Services/** (service interfaces and implementations)
8. **Controllers/PatientsController.cs**
9. **Controllers/DoctorsController.cs**
10. **Mappings/MappingProfile.cs**
11. **Validators/** (validation classes)
12. **Data/DbSeeder.cs**

---

## Testing Checklist

### API Testing with Postman
1. **Test all CRUD operations for Patients**
2. **Test all CRUD operations for Doctors**
3. **Verify proper HTTP status codes**
4. **Test validation error responses**
5. **Test Swagger documentation**

### Database Verification
1. **Check database creation**
2. **Verify seeded data**
3. **Test soft delete functionality**
4. **Verify foreign key relationships**

### Code Quality Checks
1. **Run solution build**
2. **Check for compilation errors**
3. **Verify all NuGet packages are installed**
4. **Test logging functionality**

---

## Success Criteria

✅ **Project builds without errors**
✅ **Database is created and seeded with sample data**
✅ **All CRUD APIs work for Patients and Doctors**
✅ **Swagger documentation is accessible**
✅ **Postman tests pass**
✅ **Logging is working**
✅ **Validation is implemented**
✅ **Code follows clean architecture principles**

---

## Next Steps (Future Milestones)

After completing Milestone 1, you'll be ready for:
- **Milestone 2**: Appointment Management
- **Milestone 3**: Medical Records & Lab Reports
- **Milestone 4**: Pharmacy Management
- **Milestone 5**: Billing & Finance

---

## Troubleshooting Common Issues

### Database Connection Issues
- Verify SQL Server is running
- Check connection string format
- Ensure LocalDB is installed

### Package Installation Issues
- Clear NuGet cache
- Restart Visual Studio
- Check .NET version compatibility

### Build Errors
- Check all using statements
- Verify namespace declarations
- Ensure all dependencies are installed

---

*This planning document is specifically designed for Visual Studio development workflow. Follow the steps in order for best results.*
