#!/bin/bash

# BillingService API Test Script
# This script tests the BillingService endpoints with authentication

BASE_URL="http://localhost:5007"
ADMIN_EMAIL="admin@hospital.com"
ADMIN_PASSWORD="admin123"
DOCTOR_EMAIL="doctor@hospital.com"
DOCTOR_PASSWORD="doctor123"
PATIENT_EMAIL="patient@hospital.com"
PATIENT_PASSWORD="patient123"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Function to make API calls
test_endpoint() {
    local method=$1
    local url=$2
    local headers=$3
    local data=$4
    local expected_status=$5
    local test_name=$6
    
    echo -e "${YELLOW}Testing: $test_name${NC}"
    echo "URL: $method $url"
    
    if [ -n "$data" ]; then
        response=$(curl -s -w "\n%{http_code}" -X $method "$url" -H "Content-Type: application/json" $headers -d "$data")
    else
        response=$(curl -s -w "\n%{http_code}" -X $method "$url" $headers)
    fi
    
    http_code=$(echo "$response" | tail -n1)
    body=$(echo "$response" | head -n -1)
    
    if [ "$http_code" = "$expected_status" ]; then
        echo -e "${GREEN}✓ PASSED${NC} - Status: $http_code"
        if [ "$http_code" != "204" ] && [ -n "$body" ]; then
            echo "Response: $body" | jq . 2>/dev/null || echo "Response: $body"
        fi
    else
        echo -e "${RED}✗ FAILED${NC} - Expected: $expected_status, Got: $http_code"
        echo "Response: $body"
    fi
    echo "---"
}

# Function to get JWT token
get_token() {
    local email=$1
    local password=$2
    
    response=$(curl -s -X POST "$BASE_URL/api/auth/login" \
        -H "Content-Type: application/json" \
        -d "{\"email\":\"$email\",\"password\":\"$password\"}")
    
    echo "$response" | jq -r '.token' 2>/dev/null
}

echo -e "${YELLOW}=== BillingService API Tests ===${NC}"
echo

# Get authentication tokens
echo "Getting authentication tokens..."
admin_token=$(get_token "$ADMIN_EMAIL" "$ADMIN_PASSWORD")
doctor_token=$(get_token "$DOCTOR_EMAIL" "$DOCTOR_PASSWORD")
patient_token=$(get_token "$PATIENT_EMAIL" "$PATIENT_PASSWORD")

if [ "$admin_token" = "null" ] || [ -z "$admin_token" ]; then
    echo -e "${RED}Failed to get admin token. Make sure AuthService is running.${NC}"
    exit 1
fi

if [ "$doctor_token" = "null" ] || [ -z "$doctor_token" ]; then
    echo -e "${RED}Failed to get doctor token. Make sure AuthService is running.${NC}"
    exit 1
fi

if [ "$patient_token" = "null" ] || [ -z "$patient_token" ]; then
    echo -e "${RED}Failed to get patient token. Make sure AuthService is running.${NC}"
    exit 1
fi

echo -e "${GREEN}Authentication tokens obtained successfully${NC}"
echo

# Set headers
admin_headers="-H \"Authorization: Bearer $admin_token\""
doctor_headers="-H \"Authorization: Bearer $doctor_token\""
patient_headers="-H \"Authorization: Bearer $patient_token\""

echo -e "${YELLOW}=== Testing Transactions API ===${NC}"

# Test Transactions
test_endpoint "GET" "$BASE_URL/api/transactions" "$admin_headers" "" "200" "Get All Transactions (Admin)"

# Create a transaction
transaction_data='{
    "patientId": 1,
    "transactionType": "Payment",
    "amount": 150.00,
    "description": "Consultation fee",
    "paymentMethod": "Card",
    "referenceNumber": "TXN001",
    "transactionDate": "2024-01-15T10:00:00Z"
}'

test_endpoint "POST" "$BASE_URL/api/transactions" "$admin_headers" "$transaction_data" "201" "Create Transaction (Admin)"

# Test unauthorized access
test_endpoint "POST" "$BASE_URL/api/transactions" "$patient_headers" "$transaction_data" "403" "Create Transaction (Patient - Should Fail)"

# Test patient viewing their own transactions
test_endpoint "GET" "$BASE_URL/api/transactions/patient/1" "$patient_headers" "" "200" "Get Patient Transactions (Patient)"

echo -e "${YELLOW}=== Testing Expenditures API ===${NC}"

# Test Expenditures
test_endpoint "GET" "$BASE_URL/api/expenditures" "$admin_headers" "" "200" "Get All Expenditures (Admin)"

# Create an expenditure
expenditure_data='{
    "category": "Equipment",
    "description": "Medical equipment purchase",
    "amount": 5000.00,
    "vendor": "MedSupply Inc",
    "referenceNumber": "EXP001",
    "expenditureDate": "2024-01-15T10:00:00Z"
}'

test_endpoint "POST" "$BASE_URL/api/expenditures" "$admin_headers" "$expenditure_data" "201" "Create Expenditure (Admin)"

# Test unauthorized access
test_endpoint "POST" "$BASE_URL/api/expenditures" "$patient_headers" "$expenditure_data" "403" "Create Expenditure (Patient - Should Fail)"

echo -e "${YELLOW}=== Testing Invoices API ===${NC}"

# Test Invoices
test_endpoint "GET" "$BASE_URL/api/invoices" "$admin_headers" "" "200" "Get All Invoices (Admin)"

# Create an invoice
invoice_data='{
    "patientId": 1,
    "totalAmount": 250.00,
    "description": "Medical consultation and tests",
    "invoiceDate": "2024-01-15T10:00:00Z",
    "dueDate": "2024-02-15T10:00:00Z"
}'

test_endpoint "POST" "$BASE_URL/api/invoices" "$admin_headers" "$invoice_data" "201" "Create Invoice (Admin)"

# Test invoice validation (total amount = 0)
invalid_invoice_data='{
    "patientId": 1,
    "totalAmount": 0,
    "description": "Invalid invoice",
    "invoiceDate": "2024-01-15T10:00:00Z"
}'

test_endpoint "POST" "$BASE_URL/api/invoices" "$admin_headers" "$invalid_invoice_data" "400" "Create Invoice with Zero Amount (Should Fail)"

# Test patient viewing their own invoices
test_endpoint "GET" "$BASE_URL/api/invoices/patient/1" "$patient_headers" "" "200" "Get Patient Invoices (Patient)"

# Test unauthorized access
test_endpoint "POST" "$BASE_URL/api/invoices" "$patient_headers" "$invoice_data" "403" "Create Invoice (Patient - Should Fail)"

echo -e "${YELLOW}=== Testing Search/Filter Functionality ===${NC}"

# Test transaction search
test_endpoint "GET" "$BASE_URL/api/transactions/search?patientId=1&transactionType=Payment" "$admin_headers" "" "200" "Search Transactions by Patient and Type"

# Test expenditure search
test_endpoint "GET" "$BASE_URL/api/expenditures/search?category=Equipment&minAmount=1000" "$admin_headers" "" "200" "Search Expenditures by Category and Amount"

# Test invoice search
test_endpoint "GET" "$BASE_URL/api/invoices/search?status=Pending&minAmount=100" "$admin_headers" "" "200" "Search Invoices by Status and Amount"

echo -e "${YELLOW}=== Testing Unauthorized Access ===${NC}"

# Test unauthorized access to protected endpoints
test_endpoint "GET" "$BASE_URL/api/transactions" "" "" "401" "Unauthorized Access to Transactions"
test_endpoint "GET" "$BASE_URL/api/expenditures" "" "" "401" "Unauthorized Access to Expenditures"
test_endpoint "GET" "$BASE_URL/api/invoices" "" "" "401" "Unauthorized Access to Invoices"

echo -e "${GREEN}=== All Tests Completed ===${NC}"
