#!/bin/bash

# Hospital Management System - Authentication Test Script
# This script tests all authentication functionality and API endpoints

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

# Configuration
BASE_URL_HTTPS="https://localhost:7000"
BASE_URL_HTTP="http://localhost:5000"
BASE_URL=""
SKIP_SSL="-k"

# Test results tracking
TOTAL_TESTS=0
PASSED_TESTS=0
FAILED_TESTS=0

# Function to print test results
print_test_result() {
    local test_name="$1"
    local passed="$2"
    local message="$3"
    
    TOTAL_TESTS=$((TOTAL_TESTS + 1))
    
    if [ "$passed" = "true" ]; then
        PASSED_TESTS=$((PASSED_TESTS + 1))
        echo -e "✅ $test_name"
    else
        FAILED_TESTS=$((FAILED_TESTS + 1))
        echo -e "❌ $test_name"
        if [ -n "$message" ]; then
            echo -e "   Error: $message"
        fi
    fi
}

# Function to test an endpoint
test_endpoint() {
    local method="$1"
    local url="$2"
    local headers="$3"
    local body="$4"
    local expected_status="$5"
    local test_name="$6"
    
    local response
    local status_code
    
    if [ -n "$body" ]; then
        response=$(curl -s -w "\n%{http_code}" $SKIP_SSL -X "$method" \
            -H "Content-Type: application/json" \
            -H "$headers" \
            -d "$body" \
            "$url" 2>/dev/null)
    else
        response=$(curl -s -w "\n%{http_code}" $SKIP_SSL -X "$method" \
            -H "$headers" \
            "$url" 2>/dev/null)
    fi
    
    status_code=$(echo "$response" | tail -n1)
    response_body=$(echo "$response" | head -n -1)
    
    if [ "$status_code" = "$expected_status" ]; then
        print_test_result "$test_name" "true"
        echo "$response_body"
    else
        print_test_result "$test_name" "false" "Expected $expected_status, got $status_code"
        echo ""
    fi
}

# Function to test authentication
test_authentication() {
    echo -e "\n${BLUE}🔐 Testing Authentication System${NC}"
    echo -e "${BLUE}================================${NC}"
    
    # Test 1: Test server connectivity
    echo -e "\n${YELLOW}1. Testing Server Connectivity...${NC}"
    if curl -s $SKIP_SSL "$BASE_URL_HTTPS/swagger" >/dev/null 2>&1; then
        BASE_URL="$BASE_URL_HTTPS"
        print_test_result "Server is running" "true"
    elif curl -s "$BASE_URL_HTTP/swagger" >/dev/null 2>&1; then
        BASE_URL="$BASE_URL_HTTP"
        print_test_result "Server is running (HTTP)" "true"
    else
        print_test_result "Server is running" "false" "Cannot connect to server. Make sure it's running on $BASE_URL_HTTPS or $BASE_URL_HTTP"
        return 1
    fi
    
    # Test 2: Test login with admin user
    echo -e "\n${YELLOW}2. Testing Admin Login...${NC}"
    local admin_login_data='{"username":"admin","password":"admin123"}'
    local admin_response
    admin_response=$(test_endpoint "POST" "$BASE_URL/api/auth/login" "" "$admin_login_data" "200" "Admin Login")
    
    if [ -n "$admin_response" ]; then
        local admin_token=$(echo "$admin_response" | grep -o '"token":"[^"]*"' | cut -d'"' -f4)
        local admin_headers="Authorization: Bearer $admin_token"
        
        # Test 3: Test authentication with admin token
        echo -e "\n${YELLOW}3. Testing Admin Authentication...${NC}"
        test_endpoint "GET" "$BASE_URL/api/auth/test" "$admin_headers" "" "200" "Admin Authentication Test"
        
        # Test 4: Test protected endpoints with admin token
        echo -e "\n${YELLOW}4. Testing Protected Endpoints with Admin Token...${NC}"
        test_endpoint "GET" "$BASE_URL/api/doctors" "$admin_headers" "" "200" "Get Doctors (Admin)"
        test_endpoint "GET" "$BASE_URL/api/patients" "$admin_headers" "" "200" "Get Patients (Admin)"
        test_endpoint "GET" "$BASE_URL/api/appointments" "$admin_headers" "" "200" "Get Appointments (Admin)"
    fi
    
    # Test 5: Test login with doctor user
    echo -e "\n${YELLOW}5. Testing Doctor Login...${NC}"
    local doctor_login_data='{"username":"dr.anderson","password":"doctor123"}'
    local doctor_response
    doctor_response=$(test_endpoint "POST" "$BASE_URL/api/auth/login" "" "$doctor_login_data" "200" "Doctor Login")
    
    if [ -n "$doctor_response" ]; then
        local doctor_token=$(echo "$doctor_response" | grep -o '"token":"[^"]*"' | cut -d'"' -f4)
        local doctor_headers="Authorization: Bearer $doctor_token"
        
        # Test 6: Test authentication with doctor token
        echo -e "\n${YELLOW}6. Testing Doctor Authentication...${NC}"
        test_endpoint "GET" "$BASE_URL/api/auth/test" "$doctor_headers" "" "200" "Doctor Authentication Test"
        
        # Test 7: Test doctor access to endpoints
        echo -e "\n${YELLOW}7. Testing Doctor Access to Endpoints...${NC}"
        test_endpoint "GET" "$BASE_URL/api/doctors" "$doctor_headers" "" "200" "Get Doctors (Doctor)"
        test_endpoint "GET" "$BASE_URL/api/patients" "$doctor_headers" "" "200" "Get Patients (Doctor)"
        test_endpoint "GET" "$BASE_URL/api/appointments" "$doctor_headers" "" "200" "Get Appointments (Doctor)"
    fi
    
    # Test 8: Test login with patient user
    echo -e "\n${YELLOW}8. Testing Patient Login...${NC}"
    local patient_login_data='{"username":"john.smith","password":"patient123"}'
    local patient_response
    patient_response=$(test_endpoint "POST" "$BASE_URL/api/auth/login" "" "$patient_login_data" "200" "Patient Login")
    
    if [ -n "$patient_response" ]; then
        local patient_token=$(echo "$patient_response" | grep -o '"token":"[^"]*"' | cut -d'"' -f4)
        local patient_headers="Authorization: Bearer $patient_token"
        
        # Test 9: Test authentication with patient token
        echo -e "\n${YELLOW}9. Testing Patient Authentication...${NC}"
        test_endpoint "GET" "$BASE_URL/api/auth/test" "$patient_headers" "" "200" "Patient Authentication Test"
        
        # Test 10: Test patient access to endpoints
        echo -e "\n${YELLOW}10. Testing Patient Access to Endpoints...${NC}"
        test_endpoint "GET" "$BASE_URL/api/doctors" "$patient_headers" "" "200" "Get Doctors (Patient)"
        test_endpoint "GET" "$BASE_URL/api/patients" "$patient_headers" "" "200" "Get Patients (Patient)"
        test_endpoint "GET" "$BASE_URL/api/appointments" "$patient_headers" "" "200" "Get Appointments (Patient)"
    fi
    
    # Test 11: Test unauthorized access (no token)
    echo -e "\n${YELLOW}11. Testing Unauthorized Access...${NC}"
    test_endpoint "GET" "$BASE_URL/api/doctors" "" "" "401" "Unauthorized Access to Doctors"
    test_endpoint "GET" "$BASE_URL/api/patients" "" "" "401" "Unauthorized Access to Patients"
    test_endpoint "GET" "$BASE_URL/api/appointments" "" "" "401" "Unauthorized Access to Appointments"
    
    # Test 12: Test invalid token
    echo -e "\n${YELLOW}12. Testing Invalid Token...${NC}"
    test_endpoint "GET" "$BASE_URL/api/auth/test" "Authorization: Bearer invalid-token-12345" "" "401" "Invalid Token Test"
    
    # Test 13: Test user registration
    echo -e "\n${YELLOW}13. Testing User Registration...${NC}"
    local register_data='{"username":"testuser","email":"testuser@example.com","password":"testpass123","role":"Patient"}'
    local register_response
    register_response=$(test_endpoint "POST" "$BASE_URL/api/auth/register" "" "$register_data" "201" "User Registration")
    
    if [ -n "$register_response" ]; then
        local test_user_token=$(echo "$register_response" | grep -o '"token":"[^"]*"' | cut -d'"' -f4)
        local test_user_headers="Authorization: Bearer $test_user_token"
        
        # Test 14: Test new user authentication
        echo -e "\n${YELLOW}14. Testing New User Authentication...${NC}"
        test_endpoint "GET" "$BASE_URL/api/auth/test" "$test_user_headers" "" "200" "New User Authentication Test"
    fi
}

# Function to show test summary
show_test_summary() {
    echo -e "\n${BLUE}📊 Test Summary${NC}"
    echo -e "${BLUE}===============${NC}"
    echo -e "Total Tests: $TOTAL_TESTS"
    echo -e "${GREEN}Passed: $PASSED_TESTS${NC}"
    echo -e "${RED}Failed: $FAILED_TESTS${NC}"
    
    local success_rate=$((PASSED_TESTS * 100 / TOTAL_TESTS))
    echo -e "\nSuccess Rate: $success_rate%"
    
    if [ $FAILED_TESTS -eq 0 ]; then
        echo -e "\n${GREEN}🎉 All tests passed! Authentication system is working correctly.${NC}"
    else
        echo -e "\n${YELLOW}⚠️  Some tests failed. Please check the authentication configuration.${NC}"
    fi
}

# Main execution
echo -e "${CYAN}🏥 Hospital Management System - Authentication Test${NC}"
echo -e "${CYAN}=================================================${NC}"
echo -e "Base URL (HTTPS): $BASE_URL_HTTPS"
echo -e "Base URL (HTTP): $BASE_URL_HTTP"
echo -e "Skip SSL: $SKIP_SSL"

test_authentication
show_test_summary

echo -e "\n${YELLOW}🔧 Troubleshooting Tips:${NC}"
echo -e "1. Make sure the server is running: dotnet run"
echo -e "2. Check database connection in appsettings.json"
echo -e "3. Verify JWT configuration in appsettings.json"
echo -e "4. Check logs in the logs/ directory for detailed error messages"
