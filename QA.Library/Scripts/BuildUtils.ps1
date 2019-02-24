# ####################################################################################################
# Read the environment variables from the current build.
# ####################################################################################################
$sourceDirectory = $Env:TF_BUILD_SOURCESDIRECTORY
$sourceBranchName = $Env:BUILD_SOURCEBRANCHNAME
$version = [regex]::matches($Env:BUILD_BUILDNUMBER,"\d+\.\d+\.\d+\.\d+")
$parsedVersion = [Version]::Parse($version)

# ####################################################################################################
# Update all assembly info files with the correct version.
# ####################################################################################################
$fileNames = Get-ChildItem -Path $sourceDirectory -Recurse -Filter "*SolutionAssemblyInfo.cs" | % { $_.FullName }
if ($fileNames)
{
	foreach ($fileName in $fileNames)
	{
		Write-Host "Updating $fileName to $version."
		$content = Get-Content $fileName

		$content = $content -replace "AssemblyVersion\(`"(\d+)\.(\d+)\.\d+\.(\d+)`"\)\]`$", "AssemblyVersion(`"$($parsedVersion.Major).$($parsedVersion.Minor).0.0`")]"		
		$content = $content -replace "AssemblyFileVersion\(`"(\d+)\.(\d+)\.(\d+)\.(\d+)`"\)\]`$", "AssemblyFileVersion(`"$($parsedVersion.Major).$($parsedVersion.Minor).$($parsedVersion.Build).0`")]"
		$content = $content -replace "AssemblyInformationalVersion\(`"(\d+)\.(\d+)\.(\d+)\.(\d+)`"\)\]`$", "AssemblyInformationalVersion(`"$($parsedVersion.Major).$($parsedVersion.Minor).$($parsedVersion.Build).0`")]"

		$content | Out-File $fileName
	}
}
else
{
	Write-Warning "No AssemblyInfo files found."
}

# ####################################################################################################
# Update all .nuspec files with the correct version (including prerelease suffix).
# ####################################################################################################
$fileNames = Get-ChildItem -Path $sourceDirectory -Recurse -Filter "*.nuspec" | % { $_.FullName }
if ($fileNames)
{
	if ($sourceBranchName -ne "master")
	{
		$version = "$version-$sourceBranchName"
	}

	foreach ($fileName in $fileNames)
	{
		Write-Host "Updating $fileName to $version."
		$content = Get-Content $fileName

		$content = $content -replace "\`$fullVersion\`$", $version
		$content = $content -replace "\`$minVersion\`$", $version

		$content | Out-File $fileName
	}
}
else
{
	Write-Warning "No NuSpec files found."
}

# ####################################################################################################
# Delete old .nupkg files.
# ####################################################################################################
$fileNames = Get-ChildItem -Path $sourceDirectory -Filter "*.nupkg" | % { $_.FullName }
if ($fileNames)
{
	foreach ($fileName in $fileNames)
	{
		Write-Host "Deleting $fileName"
		Remove-Item $fileName
	}
}
