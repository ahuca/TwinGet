#Requires -RunAsAdministrator

$NvmVersion = "1.1.12"
$NodeVersion = "20.9.0"
$PwshVersion = "7.4.0"

function Test-Command {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string]$Command,
        [Parameter(Mandatory = $true, Position = 1)]
        [string]$ThrowMessage
    )
    
    if (!(Get-Command $Command -ErrorAction SilentlyContinue)) {
        throw $ThrowMessage
    }
}


"choco" | Test-Command "choco is not installed on this system. See https://chocolatey.org/install"

$RefreshEnv = "$($env:ChocolateyInstall)\bin\RefreshEnv.cmd"

# Install PowerShell
choco install pwsh --version=$PwshVersion -y

# Install NVM and node
choco install nvm --version=$NvmVersion -y
$RefreshEnv
nvm install $NodeVersion
nvm use $NodeVersion
$RefreshEnv

# Restore node packages
npm ci
