# 🔧 Database Seeding Troubleshooting Guide

## 🚨 **Issue: Doctor Data Not Going to Database**

### **Root Cause Analysis**
The error shows: `Unable to connect to any of the specified MySQL hosts`

This means the database seeding is failing because:
1. **MySQL server is not running**
2. **Database connection string is incorrect**
3. **Database doesn't exist**
4. **MySQL service needs to be started**

## 🔍 **Step-by-Step Diagnosis**

### **Step 1: Check MySQL Server Status**
```bash
# Check if MySQL is running
net start | findstr mysql

# Or check services
services.msc
# Look for "MySQL80" service
```

### **Step 2: Start MySQL Server**
```bash
# Method 1: Command Line (Run as Administrator)
net start mysql80

# Method 2: Services Manager
# 1. Press Win + R
# 2. Type "services.msc"
# 3. Find "MySQL80" service
# 4. Right-click → Start

# Method 3: MySQL Workbench
# 1. Open MySQL Workbench
# 2. Try to connect to localhost:3306
```

### **Step 3: Verify Database Exists**
```sql
-- Connect to MySQL and run:
SHOW DATABASES;

-- If HospitalManagementDB doesn't exist, create it:
CREATE DATABASE HospitalManagementDB;
USE HospitalManagementDB;
```

### **Step 4: Test Connection String**
```bash
# Test connection from command line
mysql -u root -p -e "SELECT 1;"

# If prompted for password, enter your MySQL root password
# If no password, just press Enter
```

## 🛠️ **Solutions**

### **Solution 1: Fix Connection String**

#### **Current Connection String (Problematic):**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=HospitalManagementDB;Uid=root;Pwd=;"
}
```

#### **Updated Connection String (Fixed):**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=HospitalManagementDB;Uid=root;Pwd=your_password_here;"
}
```

**Replace `your_password_here` with your actual MySQL root password.**

### **Solution 2: Create Database Manually**
```sql
-- Run in MySQL Workbench or command line:
DROP DATABASE IF EXISTS HospitalManagementDB;
CREATE DATABASE HospitalManagementDB;
USE HospitalManagementDB;

-- Verify database was created
SHOW DATABASES;
```

### **Solution 3: Update DbContext Configuration**

#### **Add Retry Policy to Program.cs:**
```csharp
// In Microservices/AuthService/Program.cs
builder.Services.AddDbContext<HospitalDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 0)),
    mySqlOptions => mySqlOptions.EnableRetryOnFailure(
        maxRetryCount: 3,
        maxRetryDelay: TimeSpan.FromSeconds(30),
        errorNumbersToAdd: null)));
```

### **Solution 4: Manual Database Migration**

#### **Step 1: Create Migration**
```bash
cd Microservices\AuthService
dotnet ef migrations add InitialCreate
```

#### **Step 2: Update Database**
```bash
dotnet ef database update
```

#### **Step 3: Run Application**
```bash
dotnet run
```

## 🧪 **Testing Database Seeding**

### **Test 1: Check Database Connection**
```bash
# Run the application
cd Microservices\AuthService
dotnet run

# Check logs for database connection messages
# Look for: "Database connection successful" or similar
```

### **Test 2: Verify Data in Database**
```sql
-- Connect to MySQL and check tables:
USE HospitalManagementDB;

-- Check if tables exist
SHOW TABLES;

-- Check if data was seeded
SELECT COUNT(*) FROM Users;
SELECT COUNT(*) FROM Doctors;
SELECT COUNT(*) FROM Patients;
SELECT COUNT(*) FROM Appointments;
```

### **Test 3: Check Application Logs**
```bash
# Check logs folder
cd Microservices\AuthService/logs
# Look for error messages or success messages
```

## 🔧 **Complete Fix Process**

### **Step 1: Start MySQL Server**
```bash
# Run as Administrator
net start mysql80
```

### **Step 2: Create Database**
```sql
CREATE DATABASE HospitalManagementDB;
```

### **Step 3: Update Connection String**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=HospitalManagementDB;Uid=root;Pwd=your_password;"
  }
}
```

### **Step 4: Create Migration**
```bash
cd Microservices\AuthService
dotnet ef migrations add InitialCreate
```

### **Step 5: Update Database**
```bash
dotnet ef database update
```

### **Step 6: Run Application**
```bash
dotnet run
```

### **Step 7: Verify Seeding**
```sql
-- Check if data was seeded
SELECT * FROM Doctors;
SELECT * FROM Patients;
SELECT * FROM Users;
```

## 🚨 **Common Error Messages & Solutions**

### **Error 1: "Unable to connect to any of the specified MySQL hosts"**
**Solution**: Start MySQL server

### **Error 2: "Access denied for user 'root'@'localhost'"
**Solution**: Check password in connection string

### **Error 3: "Unknown database 'HospitalManagementDB'"
**Solution**: Create database manually

### **Error 4: "Table doesn't exist"
**Solution**: Run migrations first

## 📝 **Verification Checklist**

- ✅ MySQL server is running
- ✅ Database `HospitalManagementDB` exists
- ✅ Connection string is correct
- ✅ Migrations are applied
- ✅ Application starts without errors
- ✅ Data appears in database tables

## 🎯 **Quick Fix Commands**

```bash
# 1. Start MySQL (Run as Administrator)
net start mysql80

# 2. Create database
mysql -u root -p -e "CREATE DATABASE HospitalManagementDB;"

# 3. Run migrations
cd Microservices\AuthService
dotnet ef database update

# 4. Run application
dotnet run

# 5. Check data
mysql -u root -p -e "USE HospitalManagementDB; SELECT COUNT(*) FROM Doctors;"
```

## 🔍 **Debug Information**

### **Check MySQL Service Status:**
```bash
sc query mysql80
```

### **Check MySQL Port:**
```bash
netstat -an | findstr 3306
```

### **Check Application Logs:**
```bash
# Look in logs folder for detailed error messages
cd Microservices\AuthService/logs
type hospital-management-*.log
```

---

**Once you fix the MySQL connection, the database seeding will work and doctor data will be saved to the database! 🚀**

