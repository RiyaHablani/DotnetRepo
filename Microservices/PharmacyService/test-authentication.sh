#!/bin/bash

# Pharmacy Service API Test Script
# This script tests the Pharmacy Service endpoints

BASE_URL="http://localhost:5000"
AUTH_TOKEN=""

echo "=== Pharmacy Service API Tests ==="
echo ""

# Function to get auth token (you'll need to implement this endpoint)
get_auth_token() {
    echo "Getting authentication token..."
    # This would typically call your auth service
    # For now, we'll use a placeholder
    AUTH_TOKEN="your-jwt-token-here"
    echo "Token obtained: ${AUTH_TOKEN:0:20}..."
}

# Function to make authenticated requests
make_request() {
    local method=$1
    local endpoint=$2
    local data=$3
    
    if [ -n "$data" ]; then
        curl -X $method \
             -H "Authorization: Bearer $AUTH_TOKEN" \
             -H "Content-Type: application/json" \
             -d "$data" \
             "$BASE_URL$endpoint"
    else
        curl -X $method \
             -H "Authorization: Bearer $AUTH_TOKEN" \
             -H "Content-Type: application/json" \
             "$BASE_URL$endpoint"
    fi
}

echo "1. Testing Get All Medicines..."
make_request "GET" "/api/medicines"
echo -e "\n"

echo "2. Testing Get Unexpired Medicines..."
make_request "GET" "/api/medicines/unexpired"
echo -e "\n"

echo "3. Testing Search Medicines by Disease Type..."
make_request "GET" "/api/medicines/disease-type/Diabetes"
echo -e "\n"

echo "4. Testing Search Medicines with Query Parameters..."
make_request "GET" "/api/medicines/search?name=Paracetamol&includeExpired=false"
echo -e "\n"

echo "5. Testing Get All Prescriptions..."
make_request "GET" "/api/prescriptions"
echo -e "\n"

echo "6. Testing Get Prescriptions by Patient ID..."
make_request "GET" "/api/prescriptions/patient/1"
echo -e "\n"

echo "7. Testing Get Prescriptions by Status..."
make_request "GET" "/api/prescriptions/status/Pending"
echo -e "\n"

echo "8. Testing Create Medicine (requires Admin/Pharmacist role)..."
make_request "POST" "/api/medicines" '{
    "name": "Test Medicine",
    "description": "Test description",
    "diseaseType": "Test",
    "price": 10.50,
    "quantity": 50,
    "expiryDate": "2025-12-31T23:59:59Z",
    "manufacturer": "Test Manufacturer",
    "dosageForm": "Tablet",
    "dosageStrength": "100mg"
}'
echo -e "\n"

echo "9. Testing Create Prescription (requires Doctor/Admin role)..."
make_request "POST" "/api/prescriptions" '{
    "patientId": 1,
    "doctorId": 1,
    "prescriptionDate": "2024-01-15T10:00:00Z",
    "notes": "Test prescription",
    "patientMedicines": [
        {
            "medicineId": 1,
            "quantity": 10,
            "instructions": "Take with food",
            "dosage": "1 tablet",
            "frequency": "Twice daily",
            "duration": "5 days"
        }
    ]
}'
echo -e "\n"

echo "10. Testing Update Prescription Status (requires Pharmacist/Admin role)..."
make_request "PUT" "/api/prescriptions/1/status" '{
    "status": "Filled",
    "filledBy": "John Pharmacist"
}'
echo -e "\n"

echo "=== Test completed ==="
