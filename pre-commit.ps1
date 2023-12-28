$Configuration = "Debug"

Write-Host "Running pre-commit"
& "$PSScriptRoot\build.ps1" -Configuration $Configuration
dotnet csharpier (Resolve-Path $PSScriptRoot)
