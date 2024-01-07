$Configuration = "Debug"

Write-Host "Running pre-commit"
& "$PSScriptRoot\build.ps1" -Configuration $Configuration
dotnet format (Resolve-Path "$PSScriptRoot\TwinGet.sln")
dotnet csharpier (Resolve-Path $PSScriptRoot)
