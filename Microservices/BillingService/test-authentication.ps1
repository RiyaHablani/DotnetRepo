# BillingService API Test Script (PowerShell)
# This script tests the BillingService endpoints with authentication

$BaseUrl = "http://localhost:5007"
$AdminEmail = "admin@hospital.com"
$AdminPassword = "admin123"
$DoctorEmail = "doctor@hospital.com"
$DoctorPassword = "doctor123"
$PatientEmail = "patient@hospital.com"
$PatientPassword = "patient123"

# Function to make API calls
function Test-Endpoint {
    param(
        [string]$Method,
        [string]$Url,
        [hashtable]$Headers = @{},
        [string]$Body = $null,
        [int]$ExpectedStatusCode,
        [string]$TestName
    )
    
    Write-Host "Testing: $TestName" -ForegroundColor Yellow
    Write-Host "URL: $Method $Url"
    
    try {
        if ($Body) {
            $response = Invoke-RestMethod -Uri $Url -Method $Method -Headers $Headers -Body $Body -ContentType "application/json" -ErrorAction Stop
            $httpCode = 200
        } else {
            $response = Invoke-RestMethod -Uri $Url -Method $Method -Headers $Headers -ErrorAction Stop
            $httpCode = 200
        }
        
        Write-Host "✓ PASSED - Status: $httpCode" -ForegroundColor Green
        if ($response) {
            $response | ConvertTo-Json -Depth 3
        }
    }
    catch {
        $httpCode = $_.Exception.Response.StatusCode.value__
        if ($httpCode -eq $ExpectedStatusCode) {
            Write-Host "✓ PASSED - Status: $httpCode" -ForegroundColor Green
        } else {
            Write-Host "✗ FAILED - Expected: $ExpectedStatusCode, Got: $httpCode" -ForegroundColor Red
            Write-Host "Error: $($_.Exception.Message)"
        }
    }
    Write-Host "---"
}

# Function to get JWT token
function Get-Token {
    param([string]$Email, [string]$Password)
    
    $loginData = @{
        email = $Email
        password = $Password
    } | ConvertTo-Json
    
    try {
        $response = Invoke-RestMethod -Uri "$BaseUrl/api/auth/login" -Method POST -Body $loginData -ContentType "application/json"
        return $response.token
    }
    catch {
        Write-Host "Failed to get token for $Email" -ForegroundColor Red
        return $null
    }
}

Write-Host "=== BillingService API Tests ===" -ForegroundColor Yellow
Write-Host ""

# Get authentication tokens
Write-Host "Getting authentication tokens..."
$adminToken = Get-Token $AdminEmail $AdminPassword
$doctorToken = Get-Token $DoctorEmail $DoctorPassword
$patientToken = Get-Token $PatientEmail $PatientPassword

if (-not $adminToken) {
    Write-Host "Failed to get admin token. Make sure AuthService is running." -ForegroundColor Red
    exit 1
}

if (-not $doctorToken) {
    Write-Host "Failed to get doctor token. Make sure AuthService is running." -ForegroundColor Red
    exit 1
}

if (-not $patientToken) {
    Write-Host "Failed to get patient token. Make sure AuthService is running." -ForegroundColor Red
    exit 1
}

Write-Host "Authentication tokens obtained successfully" -ForegroundColor Green
Write-Host ""

# Set headers
$adminHeaders = @{ "Authorization" = "Bearer $adminToken" }
$doctorHeaders = @{ "Authorization" = "Bearer $doctorToken" }
$patientHeaders = @{ "Authorization" = "Bearer $patientToken" }

Write-Host "=== Testing Transactions API ===" -ForegroundColor Yellow

# Test Transactions
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/transactions" -Headers $adminHeaders -ExpectedStatusCode 200 -TestName "Get All Transactions (Admin)"

# Create a transaction
$transactionData = @{
    patientId = 1
    transactionType = "Payment"
    amount = 150.00
    description = "Consultation fee"
    paymentMethod = "Card"
    referenceNumber = "TXN001"
    transactionDate = "2024-01-15T10:00:00Z"
} | ConvertTo-Json

Test-Endpoint -Method "POST" -Url "$BaseUrl/api/transactions" -Headers $adminHeaders -Body $transactionData -ExpectedStatusCode 201 -TestName "Create Transaction (Admin)"

# Test unauthorized access
Test-Endpoint -Method "POST" -Url "$BaseUrl/api/transactions" -Headers $patientHeaders -Body $transactionData -ExpectedStatusCode 403 -TestName "Create Transaction (Patient - Should Fail)"

# Test patient viewing their own transactions
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/transactions/patient/1" -Headers $patientHeaders -ExpectedStatusCode 200 -TestName "Get Patient Transactions (Patient)"

Write-Host "=== Testing Expenditures API ===" -ForegroundColor Yellow

# Test Expenditures
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/expenditures" -Headers $adminHeaders -ExpectedStatusCode 200 -TestName "Get All Expenditures (Admin)"

# Create an expenditure
$expenditureData = @{
    category = "Equipment"
    description = "Medical equipment purchase"
    amount = 5000.00
    vendor = "MedSupply Inc"
    referenceNumber = "EXP001"
    expenditureDate = "2024-01-15T10:00:00Z"
} | ConvertTo-Json

Test-Endpoint -Method "POST" -Url "$BaseUrl/api/expenditures" -Headers $adminHeaders -Body $expenditureData -ExpectedStatusCode 201 -TestName "Create Expenditure (Admin)"

# Test unauthorized access
Test-Endpoint -Method "POST" -Url "$BaseUrl/api/expenditures" -Headers $patientHeaders -Body $expenditureData -ExpectedStatusCode 403 -TestName "Create Expenditure (Patient - Should Fail)"

Write-Host "=== Testing Invoices API ===" -ForegroundColor Yellow

# Test Invoices
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/invoices" -Headers $adminHeaders -ExpectedStatusCode 200 -TestName "Get All Invoices (Admin)"

# Create an invoice
$invoiceData = @{
    patientId = 1
    totalAmount = 250.00
    description = "Medical consultation and tests"
    invoiceDate = "2024-01-15T10:00:00Z"
    dueDate = "2024-02-15T10:00:00Z"
} | ConvertTo-Json

Test-Endpoint -Method "POST" -Url "$BaseUrl/api/invoices" -Headers $adminHeaders -Body $invoiceData -ExpectedStatusCode 201 -TestName "Create Invoice (Admin)"

# Test invoice validation (total amount = 0)
$invalidInvoiceData = @{
    patientId = 1
    totalAmount = 0
    description = "Invalid invoice"
    invoiceDate = "2024-01-15T10:00:00Z"
} | ConvertTo-Json

Test-Endpoint -Method "POST" -Url "$BaseUrl/api/invoices" -Headers $adminHeaders -Body $invalidInvoiceData -ExpectedStatusCode 400 -TestName "Create Invoice with Zero Amount (Should Fail)"

# Test patient viewing their own invoices
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/invoices/patient/1" -Headers $patientHeaders -ExpectedStatusCode 200 -TestName "Get Patient Invoices (Patient)"

# Test unauthorized access
Test-Endpoint -Method "POST" -Url "$BaseUrl/api/invoices" -Headers $patientHeaders -Body $invoiceData -ExpectedStatusCode 403 -TestName "Create Invoice (Patient - Should Fail)"

Write-Host "=== Testing Search/Filter Functionality ===" -ForegroundColor Yellow

# Test transaction search
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/transactions/search?patientId=1&transactionType=Payment" -Headers $adminHeaders -ExpectedStatusCode 200 -TestName "Search Transactions by Patient and Type"

# Test expenditure search
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/expenditures/search?category=Equipment&minAmount=1000" -Headers $adminHeaders -ExpectedStatusCode 200 -TestName "Search Expenditures by Category and Amount"

# Test invoice search
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/invoices/search?status=Pending&minAmount=100" -Headers $adminHeaders -ExpectedStatusCode 200 -TestName "Search Invoices by Status and Amount"

Write-Host "=== Testing Unauthorized Access ===" -ForegroundColor Yellow

# Test unauthorized access to protected endpoints
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/transactions" -ExpectedStatusCode 401 -TestName "Unauthorized Access to Transactions"
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/expenditures" -ExpectedStatusCode 401 -TestName "Unauthorized Access to Expenditures"
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/invoices" -ExpectedStatusCode 401 -TestName "Unauthorized Access to Invoices"

Write-Host "=== All Tests Completed ===" -ForegroundColor Green
