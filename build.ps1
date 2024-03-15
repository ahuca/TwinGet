[CmdletBinding()]
param (
    [Parameter()]
    [string]
    [ValidateSet("Release", "Debug")]
    $Configuration = "Debug",
    
    [Parameter()]
    [switch]
    $NoRestore,

    [Parameter()]
    [switch]
    $Test,

    [Parameter()]
    $MsBuildExe
)

. "$PSScriptRoot\common.ps1"

# if (!$MsBuildExe) {
#     $MsBuildExe = Resolve-MsBuildPath -ErrorAction Stop
# }
# else {
#     $MsBuildExe = $MsBuildExe
# }

$MsBuildExe ??= (Resolve-MsBuildPath -ErrorAction Stop)

if (!$MsBuildExe) {
    throw "Could not resolve MSBuild path."
}

$null = Test-Path $MsBuildExe -ErrorAction Stop

$solution = Join-Path -Path $PSScriptRoot -ChildPath 'TwinGet.sln'

if (-not $NoRestore) {
    dotnet restore $solution
}

& $MsBuildExe $solution -p:Configuration=$Configuration

if ($Test) {
    dotnet test --configuration $Configuration --no-build --no-restore --logger "trx;verbosity=detailed;LogFileName=test_results.trx" $PSScriptRoot\TwinGet.sln
}
