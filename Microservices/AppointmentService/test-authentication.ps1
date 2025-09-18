# Hospital Management System - Authentication Test Script
# This script tests all authentication functionality and API endpoints

param(
    [string]$BaseUrl = "https://localhost:7000",
    [string]$HttpUrl = "http://localhost:5000",
    [switch]$SkipSSL = $true
)

# Colors for output
$Red = "`e[31m"
$Green = "`e[32m"
$Yellow = "`e[33m"
$Blue = "`e[34m"
$Reset = "`e[0m"

# Test results tracking
$TestResults = @{
    Total = 0
    Passed = 0
    Failed = 0
    Tests = @()
}

function Write-TestResult {
    param($TestName, $Passed, $Message = "")
    $TestResults.Total++
    if ($Passed) {
        $TestResults.Passed++
        Write-Host "✅ $TestName" -ForegroundColor Green
    } else {
        $TestResults.Failed++
        Write-Host "❌ $TestName" -ForegroundColor Red
        if ($Message) {
            Write-Host "   Error: $Message" -ForegroundColor Red
        }
    }
    $TestResults.Tests += @{
        Name = $TestName
        Passed = $Passed
        Message = $Message
    }
}

function Test-Endpoint {
    param(
        [string]$Method,
        [string]$Url,
        [hashtable]$Headers = @{},
        [string]$Body = $null,
        [int]$ExpectedStatusCode = 200,
        [string]$TestName
    )
    
    try {
        $params = @{
            Method = $Method
            Uri = $Url
            Headers = $Headers
            ContentType = "application/json"
            SkipCertificateCheck = $SkipSSL
        }
        
        if ($Body) {
            $params.Body = $Body
        }
        
        $response = Invoke-RestMethod @params -ErrorAction Stop
        $statusCode = $response.StatusCode
        
        if ($statusCode -eq $ExpectedStatusCode) {
            Write-TestResult $TestName $true
            return $response
        } else {
            Write-TestResult $TestName $false "Expected $ExpectedStatusCode, got $statusCode"
            return $null
        }
    }
    catch {
        $statusCode = $_.Exception.Response.StatusCode.value__
        if ($statusCode -eq $ExpectedStatusCode) {
            Write-TestResult $TestName $true
            return $null
        } else {
            Write-TestResult $TestName $false "Expected $ExpectedStatusCode, got $statusCode. Error: $($_.Exception.Message)"
            return $null
        }
    }
}

function Test-Authentication {
    Write-Host "`n🔐 Testing Authentication System" -ForegroundColor Blue
    Write-Host "================================" -ForegroundColor Blue
    
    # Test 1: Test server connectivity
    Write-Host "`n1. Testing Server Connectivity..." -ForegroundColor Yellow
    try {
        $response = Invoke-RestMethod -Uri "$BaseUrl/swagger" -SkipCertificateCheck:$SkipSSL -ErrorAction Stop
        Write-TestResult "Server is running" $true
    }
    catch {
        try {
            $response = Invoke-RestMethod -Uri "$HttpUrl/swagger" -ErrorAction Stop
            $BaseUrl = $HttpUrl
            Write-TestResult "Server is running (HTTP)" $true
        }
        catch {
            Write-TestResult "Server is running" $false "Cannot connect to server. Make sure it's running on $BaseUrl or $HttpUrl"
            return
        }
    }
    
    # Test 2: Test login with admin user
    Write-Host "`n2. Testing Admin Login..." -ForegroundColor Yellow
    $loginData = @{
        username = "admin"
        password = "admin123"
    } | ConvertTo-Json
    
    $loginResponse = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/auth/login" -Body $loginData -TestName "Admin Login"
    
    if ($loginResponse) {
        $adminToken = $loginResponse.Token
        $adminHeaders = @{ "Authorization" = "Bearer $adminToken" }
        
        # Test 3: Test authentication with admin token
        Write-Host "`n3. Testing Admin Authentication..." -ForegroundColor Yellow
        $authTestResponse = Test-Endpoint -Method "GET" -Url "$BaseUrl/api/auth/test" -Headers $adminHeaders -TestName "Admin Authentication Test"
        
        # Test 4: Test protected endpoints with admin token
        Write-Host "`n4. Testing Protected Endpoints with Admin Token..." -ForegroundColor Yellow
        Test-Endpoint -Method "GET" -Url "$BaseUrl/api/doctors" -Headers $adminHeaders -TestName "Get Doctors (Admin)"
        Test-Endpoint -Method "GET" -Url "$BaseUrl/api/patients" -Headers $adminHeaders -TestName "Get Patients (Admin)"
        Test-Endpoint -Method "GET" -Url "$BaseUrl/api/appointments" -Headers $adminHeaders -TestName "Get Appointments (Admin)"
    }
    
    # Test 5: Test login with doctor user
    Write-Host "`n5. Testing Doctor Login..." -ForegroundColor Yellow
    $doctorLoginData = @{
        username = "dr.anderson"
        password = "doctor123"
    } | ConvertTo-Json
    
    $doctorLoginResponse = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/auth/login" -Body $doctorLoginData -TestName "Doctor Login"
    
    if ($doctorLoginResponse) {
        $doctorToken = $doctorLoginResponse.Token
        $doctorHeaders = @{ "Authorization" = "Bearer $doctorToken" }
        
        # Test 6: Test authentication with doctor token
        Write-Host "`n6. Testing Doctor Authentication..." -ForegroundColor Yellow
        $doctorAuthTestResponse = Test-Endpoint -Method "GET" -Url "$BaseUrl/api/auth/test" -Headers $doctorHeaders -TestName "Doctor Authentication Test"
        
        # Test 7: Test doctor access to endpoints
        Write-Host "`n7. Testing Doctor Access to Endpoints..." -ForegroundColor Yellow
        Test-Endpoint -Method "GET" -Url "$BaseUrl/api/doctors" -Headers $doctorHeaders -TestName "Get Doctors (Doctor)"
        Test-Endpoint -Method "GET" -Url "$BaseUrl/api/patients" -Headers $doctorHeaders -TestName "Get Patients (Doctor)"
        Test-Endpoint -Method "GET" -Url "$BaseUrl/api/appointments" -Headers $doctorHeaders -TestName "Get Appointments (Doctor)"
    }
    
    # Test 8: Test login with patient user
    Write-Host "`n8. Testing Patient Login..." -ForegroundColor Yellow
    $patientLoginData = @{
        username = "john.smith"
        password = "patient123"
    } | ConvertTo-Json
    
    $patientLoginResponse = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/auth/login" -Body $patientLoginData -TestName "Patient Login"
    
    if ($patientLoginResponse) {
        $patientToken = $patientLoginResponse.Token
        $patientHeaders = @{ "Authorization" = "Bearer $patientToken" }
        
        # Test 9: Test authentication with patient token
        Write-Host "`n9. Testing Patient Authentication..." -ForegroundColor Yellow
        $patientAuthTestResponse = Test-Endpoint -Method "GET" -Url "$BaseUrl/api/auth/test" -Headers $patientHeaders -TestName "Patient Authentication Test"
        
        # Test 10: Test patient access to endpoints
        Write-Host "`n10. Testing Patient Access to Endpoints..." -ForegroundColor Yellow
        Test-Endpoint -Method "GET" -Url "$BaseUrl/api/doctors" -Headers $patientHeaders -TestName "Get Doctors (Patient)"
        Test-Endpoint -Method "GET" -Url "$BaseUrl/api/patients" -Headers $patientHeaders -TestName "Get Patients (Patient)"
        Test-Endpoint -Method "GET" -Url "$BaseUrl/api/appointments" -Headers $patientHeaders -TestName "Get Appointments (Patient)"
    }
    
    # Test 11: Test unauthorized access (no token)
    Write-Host "`n11. Testing Unauthorized Access..." -ForegroundColor Yellow
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/doctors" -ExpectedStatusCode 401 -TestName "Unauthorized Access to Doctors"
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/patients" -ExpectedStatusCode 401 -TestName "Unauthorized Access to Patients"
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/appointments" -ExpectedStatusCode 401 -TestName "Unauthorized Access to Appointments"
    
    # Test 12: Test invalid token
    Write-Host "`n12. Testing Invalid Token..." -ForegroundColor Yellow
    $invalidHeaders = @{ "Authorization" = "Bearer invalid-token-12345" }
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/auth/test" -Headers $invalidHeaders -ExpectedStatusCode 401 -TestName "Invalid Token Test"
    
    # Test 13: Test user registration
    Write-Host "`n13. Testing User Registration..." -ForegroundColor Yellow
    $registerData = @{
        username = "testuser"
        email = "testuser@example.com"
        password = "testpass123"
        role = "Patient"
    } | ConvertTo-Json
    
    $registerResponse = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/auth/register" -Body $registerData -TestName "User Registration"
    
    if ($registerResponse) {
        $testUserToken = $registerResponse.Token
        $testUserHeaders = @{ "Authorization" = "Bearer $testUserToken" }
        
        # Test 14: Test new user authentication
        Write-Host "`n14. Testing New User Authentication..." -ForegroundColor Yellow
        Test-Endpoint -Method "GET" -Url "$BaseUrl/api/auth/test" -Headers $testUserHeaders -TestName "New User Authentication Test"
    }
}

function Show-TestSummary {
    Write-Host "`n📊 Test Summary" -ForegroundColor Blue
    Write-Host "===============" -ForegroundColor Blue
    Write-Host "Total Tests: $($TestResults.Total)" -ForegroundColor White
    Write-Host "Passed: $($TestResults.Passed)" -ForegroundColor Green
    Write-Host "Failed: $($TestResults.Failed)" -ForegroundColor Red
    
    if ($TestResults.Failed -gt 0) {
        Write-Host "`n❌ Failed Tests:" -ForegroundColor Red
        foreach ($test in $TestResults.Tests | Where-Object { -not $_.Passed }) {
            Write-Host "  - $($test.Name)" -ForegroundColor Red
            if ($test.Message) {
                Write-Host "    $($test.Message)" -ForegroundColor Red
            }
        }
    }
    
    $successRate = [math]::Round(($TestResults.Passed / $TestResults.Total) * 100, 2)
    Write-Host "`nSuccess Rate: $successRate%" -ForegroundColor $(if ($successRate -ge 80) { "Green" } elseif ($successRate -ge 60) { "Yellow" } else { "Red" })
    
    if ($TestResults.Failed -eq 0) {
        Write-Host "`n🎉 All tests passed! Authentication system is working correctly." -ForegroundColor Green
    } else {
        Write-Host "`n⚠️  Some tests failed. Please check the authentication configuration." -ForegroundColor Yellow
    }
}

# Main execution
Write-Host "🏥 Hospital Management System - Authentication Test" -ForegroundColor Cyan
Write-Host "=================================================" -ForegroundColor Cyan
Write-Host "Base URL: $BaseUrl" -ForegroundColor White
Write-Host "HTTP URL: $HttpUrl" -ForegroundColor White
Write-Host "Skip SSL: $SkipSSL" -ForegroundColor White

Test-Authentication
Show-TestSummary

Write-Host "`n🔧 Troubleshooting Tips:" -ForegroundColor Yellow
Write-Host "1. Make sure the server is running: dotnet run" -ForegroundColor White
Write-Host "2. Check database connection in appsettings.json" -ForegroundColor White
Write-Host "3. Verify JWT configuration in appsettings.json" -ForegroundColor White
Write-Host "4. Check logs in the logs/ directory for detailed error messages" -ForegroundColor White
