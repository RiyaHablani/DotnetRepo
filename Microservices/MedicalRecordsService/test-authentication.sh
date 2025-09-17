#!/bin/bash

# Medical Records Service API Test Script
# This script tests the Medical Records and Lab Reports endpoints

BASE_URL="http://localhost:5005"
AUTH_URL="http://localhost:5001"

echo "=== Medical Records Service API Tests ==="
echo ""

# Test 1: Get authentication token
echo "1. Getting authentication token..."
AUTH_RESPONSE=$(curl -s -X POST "$AUTH_URL/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "admin123"}')

TOKEN=$(echo $AUTH_RESPONSE | grep -o '"Token":"[^"]*"' | cut -d'"' -f4)

if [ -z "$TOKEN" ]; then
  echo "❌ Failed to get authentication token"
  echo "Response: $AUTH_RESPONSE"
  exit 1
fi

echo "✅ Authentication token obtained"
echo ""

# Test 2: Test Medical Records endpoints
echo "2. Testing Medical Records endpoints..."

# Get all medical records for patient 1
echo "   Getting medical records for patient 1..."
curl -s -X GET "$BASE_URL/api/medicalrecords/patient/1" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" | jq '.'

echo ""

# Get specific medical record
echo "   Getting medical record ID 1..."
curl -s -X GET "$BASE_URL/api/medicalrecords/1" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" | jq '.'

echo ""

# Create new medical record
echo "   Creating new medical record..."
curl -s -X POST "$BASE_URL/api/medicalrecords" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "patientId": 2,
    "doctorId": 1,
    "diagnosis": "Test Diagnosis",
    "symptoms": "Test symptoms",
    "treatment": "Test treatment",
    "notes": "Test notes",
    "recordDate": "2024-01-25T10:00:00Z"
  }' | jq '.'

echo ""

# Test 3: Test Lab Reports endpoints
echo "3. Testing Lab Reports endpoints..."

# Get all lab reports for patient 1
echo "   Getting lab reports for patient 1..."
curl -s -X GET "$BASE_URL/api/labreports/patient/1" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" | jq '.'

echo ""

# Get specific lab report
echo "   Getting lab report ID 1..."
curl -s -X GET "$BASE_URL/api/labreports/1" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" | jq '.'

echo ""

# Create new lab report
echo "   Creating new lab report..."
curl -s -X POST "$BASE_URL/api/labreports" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "patientId": 2,
    "doctorId": 1,
    "testName": "Blood Test",
    "testDescription": "Complete blood count",
    "results": "Normal",
    "status": "Completed",
    "notes": "All values within normal range",
    "testDate": "2024-01-25T09:00:00Z",
    "completedDate": "2024-01-25T09:30:00Z"
  }' | jq '.'

echo ""

# Get lab reports by status
echo "   Getting lab reports by status (Pending)..."
curl -s -X GET "$BASE_URL/api/labreports/status/Pending" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" | jq '.'

echo ""

echo "=== All tests completed ==="
