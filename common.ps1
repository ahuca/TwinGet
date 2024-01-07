$VsWherePath = "$PSScriptRoot\tools\vswhere.exe"
$VsInstallationPath = & $VsWherePath -latest -property installationPath

function Resolve-MsBuildPath {
    [CmdletBinding()]
    param (
    )
    
    if (!$VsInstallationPath) { 
        Write-Error "Could not resolve Visual Studio installation path."
        return $null
    }

    return Join-Path -Path $VsInstallationPath -ChildPath 'MSBuild\Current\Bin\MSBuild.exe'
}
