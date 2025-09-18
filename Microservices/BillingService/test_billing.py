#!/usr/bin/env python3
import jwt
import requests
import json
from datetime import datetime, timedelta

# JWT Configuration (matching the appsettings.json)
SECRET_KEY = "your-super-secret-key-for-hospital-management-system-2024"
ISSUER = "HospitalManagementSystem"
AUDIENCE = "HospitalManagementSystem"

def create_test_jwt(role="Admin", patient_id=None):
    """Create a test JWT token for testing"""
    now = datetime.utcnow()
    payload = {
        "sub": "test-user",
        "role": role,
        "iss": ISSUER,
        "aud": AUDIENCE,
        "iat": now,
        "exp": now + timedelta(hours=1)
    }
    
    if patient_id:
        payload["PatientId"] = str(patient_id)
    
    token = jwt.encode(payload, SECRET_KEY, algorithm="HS256")
    return token

def test_endpoint(url, token, method="GET", data=None):
    """Test an endpoint with JWT token"""
    headers = {
        "Authorization": f"Bearer {token}",
        "Content-Type": "application/json"
    }
    
    try:
        if method == "GET":
            response = requests.get(url, headers=headers)
        elif method == "POST":
            response = requests.post(url, headers=headers, json=data)
        elif method == "PUT":
            response = requests.put(url, headers=headers, json=data)
        elif method == "DELETE":
            response = requests.delete(url, headers=headers)
        
        print(f"{method} {url}")
        print(f"Status: {response.status_code}")
        
        if response.status_code == 200:
            try:
                json_data = response.json()
                if isinstance(json_data, list):
                    print(f"Response: {len(json_data)} items returned")
                    if json_data:
                        print(f"First item keys: {list(json_data[0].keys())}")
                else:
                    print(f"Response keys: {list(json_data.keys())}")
            except:
                print(f"Response: {response.text[:200]}...")
        else:
            print(f"Error: {response.text}")
        
        print("-" * 50)
        return response.status_code == 200
        
    except Exception as e:
        print(f"Error testing {url}: {e}")
        return False

def main():
    base_url = "http://localhost:5007"
    
    print("=== Billing Service API Testing ===")
    print()
    
    # Test with Admin role
    print("1. Testing with Admin role:")
    admin_token = create_test_jwt("Admin")
    print(f"Admin Token: {admin_token[:50]}...")
    print()
    
    # Test Transaction endpoints
    print("Testing Transaction endpoints:")
    test_endpoint(f"{base_url}/api/transactions", admin_token)
    test_endpoint(f"{base_url}/api/transactions/patient/1", admin_token)
    test_endpoint(f"{base_url}/api/transactions/search?patientId=1", admin_token)
    
    # Test Invoice endpoints
    print("Testing Invoice endpoints:")
    test_endpoint(f"{base_url}/api/invoices", admin_token)
    test_endpoint(f"{base_url}/api/invoices/patient/1", admin_token)
    
    # Test Expenditure endpoints
    print("Testing Expenditure endpoints:")
    test_endpoint(f"{base_url}/api/expenditures", admin_token)
    
    # Test creating a transaction (should work with Admin)
    print("\n2. Testing Transaction Creation:")
    transaction_data = {
        "patientId": 1,
        "amount": 150.00,
        "transactionType": "Payment",
        "description": "Test transaction via API",
        "paymentMethod": "Credit Card"
    }
    
    test_endpoint(f"{base_url}/api/transactions", admin_token, "POST", transaction_data)
    
    # Test with Patient role
    print("\n3. Testing with Patient role:")
    patient_token = create_test_jwt("Patient", patient_id=1)
    print(f"Patient Token: {patient_token[:50]}...")
    print()
    
    # Test patient-specific access
    test_endpoint(f"{base_url}/api/transactions/patient/1", patient_token)
    test_endpoint(f"{base_url}/api/transactions/patient/2", patient_token)  # Should fail
    
    print("\n=== Testing Complete ===")

if __name__ == "__main__":
    main()


