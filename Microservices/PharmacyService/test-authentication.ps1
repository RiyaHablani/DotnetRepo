# Pharmacy Service API Test Script (PowerShell)
# This script tests the Pharmacy Service endpoints

$BaseUrl = "http://localhost:5000"
$AuthToken = ""

Write-Host "=== Pharmacy Service API Tests ===" -ForegroundColor Green
Write-Host ""

# Function to get auth token (you'll need to implement this endpoint)
function Get-AuthToken {
    Write-Host "Getting authentication token..." -ForegroundColor Yellow
    # This would typically call your auth service
    # For now, we'll use a placeholder
    $script:AuthToken = "your-jwt-token-here"
    Write-Host "Token obtained: $($AuthToken.Substring(0, 20))..." -ForegroundColor Yellow
}

# Function to make authenticated requests
function Invoke-AuthenticatedRequest {
    param(
        [string]$Method,
        [string]$Endpoint,
        [string]$Body = $null
    )
    
    $headers = @{
        "Authorization" = "Bearer $AuthToken"
        "Content-Type" = "application/json"
    }
    
    if ($Body) {
        Invoke-RestMethod -Uri "$BaseUrl$Endpoint" -Method $Method -Headers $headers -Body $Body
    } else {
        Invoke-RestMethod -Uri "$BaseUrl$Endpoint" -Method $Method -Headers $headers
    }
}

Write-Host "1. Testing Get All Medicines..." -ForegroundColor Cyan
try {
    Invoke-AuthenticatedRequest -Method "GET" -Endpoint "/api/medicines"
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

Write-Host "2. Testing Get Unexpired Medicines..." -ForegroundColor Cyan
try {
    Invoke-AuthenticatedRequest -Method "GET" -Endpoint "/api/medicines/unexpired"
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

Write-Host "3. Testing Search Medicines by Disease Type..." -ForegroundColor Cyan
try {
    Invoke-AuthenticatedRequest -Method "GET" -Endpoint "/api/medicines/disease-type/Diabetes"
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

Write-Host "4. Testing Search Medicines with Query Parameters..." -ForegroundColor Cyan
try {
    Invoke-AuthenticatedRequest -Method "GET" -Endpoint "/api/medicines/search?name=Paracetamol&includeExpired=false"
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

Write-Host "5. Testing Get All Prescriptions..." -ForegroundColor Cyan
try {
    Invoke-AuthenticatedRequest -Method "GET" -Endpoint "/api/prescriptions"
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

Write-Host "6. Testing Get Prescriptions by Patient ID..." -ForegroundColor Cyan
try {
    Invoke-AuthenticatedRequest -Method "GET" -Endpoint "/api/prescriptions/patient/1"
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

Write-Host "7. Testing Get Prescriptions by Status..." -ForegroundColor Cyan
try {
    Invoke-AuthenticatedRequest -Method "GET" -Endpoint "/api/prescriptions/status/Pending"
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

Write-Host "8. Testing Create Medicine (requires Admin/Pharmacist role)..." -ForegroundColor Cyan
$medicineData = @{
    name = "Test Medicine"
    description = "Test description"
    diseaseType = "Test"
    price = 10.50
    quantity = 50
    expiryDate = "2025-12-31T23:59:59Z"
    manufacturer = "Test Manufacturer"
    dosageForm = "Tablet"
    dosageStrength = "100mg"
} | ConvertTo-Json

try {
    Invoke-AuthenticatedRequest -Method "POST" -Endpoint "/api/medicines" -Body $medicineData
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

Write-Host "9. Testing Create Prescription (requires Doctor/Admin role)..." -ForegroundColor Cyan
$prescriptionData = @{
    patientId = 1
    doctorId = 1
    prescriptionDate = "2024-01-15T10:00:00Z"
    notes = "Test prescription"
    patientMedicines = @(
        @{
            medicineId = 1
            quantity = 10
            instructions = "Take with food"
            dosage = "1 tablet"
            frequency = "Twice daily"
            duration = "5 days"
        }
    )
} | ConvertTo-Json -Depth 3

try {
    Invoke-AuthenticatedRequest -Method "POST" -Endpoint "/api/prescriptions" -Body $prescriptionData
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

Write-Host "10. Testing Update Prescription Status (requires Pharmacist/Admin role)..." -ForegroundColor Cyan
$statusData = @{
    status = "Filled"
    filledBy = "John Pharmacist"
} | ConvertTo-Json

try {
    Invoke-AuthenticatedRequest -Method "PUT" -Endpoint "/api/prescriptions/1/status" -Body $statusData
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

Write-Host "=== Test completed ===" -ForegroundColor Green
