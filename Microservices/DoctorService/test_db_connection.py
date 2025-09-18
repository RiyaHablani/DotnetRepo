#!/usr/bin/env python3
"""
Test script to verify DoctorService database connection and data persistence
"""

import requests
import json
import time

# Configuration
BASE_URL = "https://localhost:7001"  # Update port if different
API_BASE = f"{BASE_URL}/api/doctors"

def test_database_connection():
    """Test if the DoctorService can connect to database and return data"""
    print("🔍 Testing DoctorService Database Connection...")
    print("=" * 50)
    
    try:
        # Test 1: Get all doctors (should return seeded data)
        print("📋 Test 1: Getting all doctors...")
        response = requests.get(API_BASE, verify=False, timeout=10)
        
        if response.status_code == 200:
            doctors = response.json()
            print(f"✅ Success! Found {len(doctors)} doctors in database:")
            for doctor in doctors:
                print(f"   - ID: {doctor.get('id')}, Name: {doctor.get('name')}, Specialization: {doctor.get('specialization')}")
        else:
            print(f"❌ Failed to get doctors. Status: {response.status_code}")
            print(f"   Response: {response.text}")
            return False
            
        # Test 2: Create a new doctor
        print("\n📝 Test 2: Creating a new doctor...")
        new_doctor = {
            "name": "Dr. Test User",
            "specialization": "General Medicine"
        }
        
        response = requests.post(API_BASE, json=new_doctor, verify=False, timeout=10)
        
        if response.status_code == 201:
            created_doctor = response.json()
            print(f"✅ Success! Created doctor with ID: {created_doctor.get('id')}")
            print(f"   Name: {created_doctor.get('name')}")
            print(f"   Specialization: {created_doctor.get('specialization')}")
            print(f"   Created At: {created_doctor.get('createdAt')}")
        else:
            print(f"❌ Failed to create doctor. Status: {response.status_code}")
            print(f"   Response: {response.text}")
            return False
            
        # Test 3: Verify the new doctor was saved
        print("\n🔍 Test 3: Verifying new doctor was saved...")
        response = requests.get(API_BASE, verify=False, timeout=10)
        
        if response.status_code == 200:
            doctors = response.json()
            test_doctor = next((d for d in doctors if d.get('name') == "Dr. Test User"), None)
            if test_doctor:
                print(f"✅ Success! New doctor found in database with ID: {test_doctor.get('id')}")
            else:
                print("❌ New doctor not found in database!")
                return False
        else:
            print(f"❌ Failed to verify new doctor. Status: {response.status_code}")
            return False
            
        print("\n🎉 All tests passed! Database connection and data persistence working correctly.")
        return True
        
    except requests.exceptions.ConnectionError:
        print("❌ Connection Error: DoctorService is not running or not accessible")
        print("   Make sure to start the service with: dotnet run")
        return False
    except requests.exceptions.Timeout:
        print("❌ Timeout Error: Service took too long to respond")
        return False
    except Exception as e:
        print(f"❌ Unexpected error: {str(e)}")
        return False

def check_service_status():
    """Check if the service is running"""
    print("🔍 Checking if DoctorService is running...")
    try:
        response = requests.get(f"{BASE_URL}/swagger", verify=False, timeout=5)
        if response.status_code == 200:
            print("✅ DoctorService is running and accessible")
            return True
        else:
            print(f"⚠️  Service responded with status: {response.status_code}")
            return False
    except:
        print("❌ DoctorService is not running or not accessible")
        print("   Start it with: dotnet run")
        return False

if __name__ == "__main__":
    print("🏥 DoctorService Database Connection Test")
    print("=" * 50)
    
    if check_service_status():
        test_database_connection()
    else:
        print("\n💡 To start the service:")
        print("   1. Open PowerShell as Administrator")
        print("   2. Start MySQL: net start mysql80")
        print("   3. Run: dotnet run")
        print("   4. Then run this test again")
