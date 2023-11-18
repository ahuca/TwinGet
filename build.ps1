$vsInstallationPath = .\tools\vswhere.exe -latest -property installationPath

$msBuildPath = Join-Path -Path $vsInstallationPath -ChildPath 'MSBuild\Current\Bin\MSBuild.exe'

$null = Test-Path $msBuildPath -ErrorAction Stop

$null = Get-Command nuget -ErrorAction Stop

$solution = Join-Path -Path $PSScriptRoot -ChildPath 'TwinGet.sln'

nuget restore
& $msBuildPath $solution
