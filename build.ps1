$vsInstallationPath = .\tools\vswhere.exe -latest -property installationPath

$msBuildPath = Join-Path -Path $vsInstallationPath -ChildPath 'MSBuild\Current\Bin\MSBuild.exe'

$null = Test-Path $msBuildPath -ErrorAction Stop

& $msBuildPath
