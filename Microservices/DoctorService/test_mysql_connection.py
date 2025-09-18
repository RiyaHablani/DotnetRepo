#!/usr/bin/env python3
"""
Test MySQL connection and database setup for DoctorService
"""

import mysql.connector
from mysql.connector import Error
import sys

def test_mysql_connection():
    """Test MySQL connection and create database if needed"""
    print("🔍 Testing MySQL Connection for DoctorService...")
    print("=" * 50)
    
    try:
        # Test connection to MySQL server
        print("1. Testing MySQL server connection...")
        connection = mysql.connector.connect(
            host='localhost',
            user='root',
            password=''  # Empty password as per your connection string
        )
        
        if connection.is_connected():
            print("✅ MySQL server is running and accessible")
            
            # Check if database exists
            print("\n2. Checking if HospitalManagementDB exists...")
            cursor = connection.cursor()
            cursor.execute("SHOW DATABASES LIKE 'HospitalManagementDB'")
            result = cursor.fetchone()
            
            if result:
                print("✅ HospitalManagementDB database exists")
                
                # Check if Doctors table exists
                print("\n3. Checking if Doctors table exists...")
                cursor.execute("USE HospitalManagementDB")
                cursor.execute("SHOW TABLES LIKE 'Doctors'")
                table_result = cursor.fetchone()
                
                if table_result:
                    print("✅ Doctors table exists")
                    
                    # Check if table has data
                    print("\n4. Checking if Doctors table has data...")
                    cursor.execute("SELECT COUNT(*) FROM Doctors")
                    count = cursor.fetchone()[0]
                    print(f"📊 Doctors table has {count} records")
                    
                    if count > 0:
                        print("\n5. Sample data in Doctors table:")
                        cursor.execute("SELECT Id, Name, Specialization, IsActive FROM Doctors LIMIT 5")
                        doctors = cursor.fetchall()
                        for doctor in doctors:
                            print(f"   - ID: {doctor[0]}, Name: {doctor[1]}, Specialization: {doctor[2]}, Active: {doctor[3]}")
                    else:
                        print("❌ Doctors table is empty - data seeding failed")
                        return False
                else:
                    print("❌ Doctors table does not exist")
                    return False
            else:
                print("❌ HospitalManagementDB database does not exist")
                return False
                
        return True
        
    except Error as e:
        print(f"❌ MySQL Connection Error: {e}")
        if e.errno == 2003:
            print("💡 MySQL server is not running!")
            print("   Solution: Start MySQL service as Administrator:")
            print("   1. Open PowerShell as Administrator")
            print("   2. Run: net start mysql80")
            print("   3. Then run this test again")
        elif e.errno == 1045:
            print("💡 Authentication failed!")
            print("   Solution: Check your MySQL username/password")
        return False
        
    except Exception as e:
        print(f"❌ Unexpected error: {e}")
        return False
        
    finally:
        if 'connection' in locals() and connection.is_connected():
            connection.close()
            print("\n🔌 MySQL connection closed")

def create_database_and_table():
    """Create database and table if they don't exist"""
    print("\n🔧 Creating database and table...")
    
    try:
        connection = mysql.connector.connect(
            host='localhost',
            user='root',
            password=''
        )
        cursor = connection.cursor()
        
        # Create database
        cursor.execute("CREATE DATABASE IF NOT EXISTS HospitalManagementDB")
        print("✅ Database created/verified")
        
        # Use database
        cursor.execute("USE HospitalManagementDB")
        
        # Create Doctors table
        create_table_sql = """
        CREATE TABLE IF NOT EXISTS Doctors (
            Id INT AUTO_INCREMENT PRIMARY KEY,
            Name VARCHAR(100) NOT NULL,
            Specialization VARCHAR(50) NOT NULL,
            IsActive BOOLEAN NOT NULL,
            CreatedAt DATETIME NOT NULL,
            UpdatedAt DATETIME NOT NULL
        )
        """
        cursor.execute(create_table_sql)
        print("✅ Doctors table created/verified")
        
        # Insert sample data
        insert_data_sql = """
        INSERT IGNORE INTO Doctors (Name, Specialization, IsActive, CreatedAt, UpdatedAt) VALUES
        ('Dr. James Anderson', 'Cardiology', TRUE, NOW(), NOW()),
        ('Dr. Lisa Martinez', 'Pediatrics', TRUE, NOW(), NOW()),
        ('Dr. David Thompson', 'Orthopedics', TRUE, NOW(), NOW())
        """
        cursor.execute(insert_data_sql)
        connection.commit()
        print("✅ Sample data inserted")
        
        return True
        
    except Error as e:
        print(f"❌ Error creating database/table: {e}")
        return False
    finally:
        if 'connection' in locals() and connection.is_connected():
            connection.close()

if __name__ == "__main__":
    print("🏥 DoctorService MySQL Database Test")
    print("=" * 50)
    
    if not test_mysql_connection():
        print("\n🔧 Attempting to fix database issues...")
        create_database_and_table()
        print("\n🔄 Re-testing connection...")
        test_mysql_connection()
