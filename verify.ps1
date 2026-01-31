$baseUrl = "http://localhost:5196" # Adjust port as needed
$adminEmail = "admin@test.com"
$customerEmail = "customer@test.com"
$password = "Password123!"

# Function to post
function Post-Request($uri, $body, $token = $null) {
    $headers = @{}
    if ($token) { $headers["Authorization"] = "Bearer $token" }
    try {
        $response = Invoke-RestMethod -Uri "$baseUrl$uri" -Method Post -Body ($body | ConvertTo-Json) -ContentType "application/json" -Headers $headers -ErrorAction Stop
        return $response
    }
    catch {
        Write-Host "Error calling $uri : $_" -ForegroundColor Red
        return $_.Exception.Response
    }
}

# Function to get
function Get-Request($uri, $token = $null) {
    $headers = @{}
    if ($token) { $headers["Authorization"] = "Bearer $token" }
    try {
        $response = Invoke-RestMethod -Uri "$baseUrl$uri" -Method Get -Headers $headers -ErrorAction Stop
        return $response
    }
    catch {
        Write-Host "Error calling $uri : $_" -ForegroundColor Red
        return $_.Exception.Response
    }
}

Write-Host "1. Register Admin" -ForegroundColor Cyan
$adminReg = @{ name = "Admin"; email = $adminEmail; password = $password; role = "Admin" }
Post-Request "/api/auth/register" $adminReg

Write-Host "2. Login Admin" -ForegroundColor Cyan
$adminLogin = @{ email = $adminEmail; password = $password }
$adminRes = Post-Request "/api/auth/login" $adminLogin
$adminToken = $adminRes.token
Write-Host "Admin Token: $adminToken" -ForegroundColor Green

Write-Host "3. Register Customer" -ForegroundColor Cyan
$custReg = @{ name = "Customer"; email = $customerEmail; password = $password; role = "Customer" }
Post-Request "/api/auth/register" $custReg

Write-Host "4. Login Customer" -ForegroundColor Cyan
$custLogin = @{ email = $customerEmail; password = $password }
$custRes = Post-Request "/api/auth/login" $custLogin
$custToken = $custRes.token
Write-Host "Customer Token: $custToken" -ForegroundColor Green

Write-Host "5. Create Book as Admin" -ForegroundColor Cyan
$book = @{ title = "New Book" }
Post-Request "/api/books" $book $adminToken

Write-Host "6. Create Book as Customer (Should Fail)" -ForegroundColor Cyan
Post-Request "/api/books" $book $custToken

Write-Host "7. Get Books" -ForegroundColor Cyan
$books = Get-Request "/api/books?page=1&pageSize=5" $custToken
Write-Output $books

Write-Host "Verification Complete" -ForegroundColor Magenta
