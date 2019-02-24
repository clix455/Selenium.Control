# ####################################################################################################
# Mock the environment variables from the current build.
# ####################################################################################################

$Env:TF_BUILD_SOURCESDIRECTORY = "$PSScriptRoot\.." | Resolve-Path
$Env:BUILD_SOURCEBRANCHNAME = 'master'
$Env:BUILD_BUILDNUMBER = 'QA.Library_2.0.16.0'

Push-Location $PSScriptRoot
& .\BuildUtils.ps1

$nugetFiles = Get-ChildItem -Path $Env:TF_BUILD_SOURCESDIRECTORY -Recurse -Filter "Selenium*.csproj" | % { $_.FullName }

foreach ($fileName in $nugetFiles)
{
	Write-Host "Packaging $fileName ..."
		
    nuget pack $fileName -outputDirectory "C:\packages"
}