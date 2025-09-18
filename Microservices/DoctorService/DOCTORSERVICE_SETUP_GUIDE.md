# DoctorService Setup and Testing Guide

## Problem Fixed
The DoctorService was not properly connected to MySQL database. The issues were:
1. ❌ No Entity Framework configuration in Program.cs
2. ❌ Controller was using static list instead of database
3. ❌ No database seeding
4. ❌ Missing database context registration

## What Was Fixed
✅ **Program.cs**: Added Entity Framework configuration and database seeding
✅ **DoctorsController**: Updated to use database instead of static list
✅ **Database Seeding**: Added automatic data population on startup
✅ **CRUD Operations**: Added full Create, Read, Update, Delete operations

## How to Run DoctorService

### 1. Prerequisites
- MySQL Server running on localhost
- .NET 8.0 SDK installed
- Database `HospitalManagementDB` created

### 2. Database Setup
```sql
-- Run this in MySQL Workbench or command line
DROP DATABASE IF EXISTS HospitalManagementDB;
CREATE DATABASE HospitalManagementDB;
USE HospitalManagementDB;
```

### 3. Update Connection String (if needed)
Edit `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HospitalManagementDB;Uid=root;Pwd=YOUR_PASSWORD;"
  }
}
```

### 4. Run the Service
```bash
cd Microservices/DoctorService
dotnet run
```

The service will:
- Automatically create the `Doctors` table
- Seed initial data (3 sample doctors)
- Start on `https://localhost:7001` (or check console for actual port)

### 5. Test the API

#### Option A: Use the Python test script
```bash
cd Microservices/DoctorService
python test_doctors.py
```

#### Option B: Manual testing with curl
```bash
# Get all doctors
curl -k https://localhost:7001/api/doctors

# Get specific doctor
curl -k https://localhost:7001/api/doctors/1

# Create new doctor
curl -k -X POST https://localhost:7001/api/doctors \
  -H "Content-Type: application/json" \
  -d '{"name":"Dr. New Doctor","specialization":"Neurology"}'
```

#### Option C: Use Swagger UI
1. Open browser: `https://localhost:7001/swagger`
2. Test the endpoints interactively

### 6. Verify Data in MySQL
```sql
-- Connect to MySQL
mysql -u root -p

-- Use the database
USE HospitalManagementDB;

-- Check if table exists
SHOW TABLES;

-- View all doctors
SELECT * FROM Doctors;

-- Check table structure
DESCRIBE Doctors;
```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/doctors` | Get all active doctors |
| GET | `/api/doctors/{id}` | Get specific doctor |
| POST | `/api/doctors` | Create new doctor |
| PUT | `/api/doctors/{id}` | Update doctor |
| DELETE | `/api/doctors/{id}` | Soft delete doctor (sets IsActive=false) |

## Sample Doctor Data Structure
```json
{
  "id": 1,
  "name": "Dr. James Anderson",
  "specialization": "Cardiology",
  "isActive": true,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

## Troubleshooting

### Issue: "Connection refused" or "Cannot connect to MySQL"
- Check if MySQL is running: `mysql -u root -p`
- Verify connection string in `appsettings.json`
- Ensure database `HospitalManagementDB` exists

### Issue: "Table doesn't exist"
- The table is created automatically on first run
- Check if the service started successfully
- Look for error messages in the console

### Issue: "No data in table"
- Data is seeded automatically on first run
- Check if there are any error messages during startup
- Verify the seeding code ran (check console logs)

### Issue: Service won't start
- Check if port 7001 is available
- Verify all NuGet packages are restored: `dotnet restore`
- Check for compilation errors: `dotnet build`

## Expected Results
After running the service, you should see:
1. ✅ Service starts without errors
2. ✅ Console shows "Database created and seeded"
3. ✅ API endpoints return data from MySQL
4. ✅ MySQL table `Doctors` contains 3 initial records
5. ✅ New doctors can be created via API
6. ✅ Data persists between service restarts
