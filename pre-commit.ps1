$Configuration = "Debug"

Write-Host "Running pre-commit"
dotnet tool restore
dotnet csharpier (Resolve-Path $PSScriptRoot)
& "$PSScriptRoot\build.ps1" -Configuration $Configuration
