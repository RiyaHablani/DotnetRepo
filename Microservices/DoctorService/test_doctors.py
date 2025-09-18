#!/usr/bin/env python3
"""
Test script for DoctorService API endpoints
Run this after starting the DoctorService to verify it's working with MySQL
"""

import requests
import json
import sys

# Configuration
BASE_URL = "https://localhost:7001"  # Update this to match your DoctorService URL
API_BASE = f"{BASE_URL}/api/doctors"

def test_doctors_api():
    """Test all DoctorService endpoints"""
    
    print("🏥 Testing DoctorService API...")
    print(f"Base URL: {API_BASE}")
    print("-" * 50)
    
    # Test 1: Get all doctors
    print("1. Testing GET /api/doctors (Get all doctors)")
    try:
        response = requests.get(API_BASE, verify=False)  # Disable SSL verification for localhost
        print(f"   Status: {response.status_code}")
        
        if response.status_code == 200:
            doctors = response.json()
            print(f"   Found {len(doctors)} doctors:")
            for doctor in doctors:
                print(f"   - ID: {doctor.get('id')}, Name: {doctor.get('name')}, Specialization: {doctor.get('specialization')}")
        else:
            print(f"   Error: {response.text}")
            
    except requests.exceptions.RequestException as e:
        print(f"   Connection Error: {e}")
        return False
    
    print()
    
    # Test 2: Get specific doctor
    print("2. Testing GET /api/doctors/1 (Get specific doctor)")
    try:
        response = requests.get(f"{API_BASE}/1", verify=False)
        print(f"   Status: {response.status_code}")
        
        if response.status_code == 200:
            doctor = response.json()
            print(f"   Doctor: {doctor.get('name')} - {doctor.get('specialization')}")
        else:
            print(f"   Error: {response.text}")
            
    except requests.exceptions.RequestException as e:
        print(f"   Connection Error: {e}")
    
    print()
    
    # Test 3: Create new doctor
    print("3. Testing POST /api/doctors (Create new doctor)")
    new_doctor = {
        "name": "Dr. Test User",
        "specialization": "General Medicine"
    }
    
    try:
        response = requests.post(API_BASE, json=new_doctor, verify=False)
        print(f"   Status: {response.status_code}")
        
        if response.status_code == 201:
            created_doctor = response.json()
            print(f"   Created Doctor: {created_doctor.get('name')} - {created_doctor.get('specialization')}")
            print(f"   ID: {created_doctor.get('id')}")
        else:
            print(f"   Error: {response.text}")
            
    except requests.exceptions.RequestException as e:
        print(f"   Connection Error: {e}")
    
    print()
    print("✅ DoctorService API testing completed!")
    print("\nTo verify data in MySQL:")
    print("1. Connect to MySQL: mysql -u root -p")
    print("2. Use database: USE HospitalManagementDB;")
    print("3. Check doctors table: SELECT * FROM Doctors;")

if __name__ == "__main__":
    test_doctors_api()
