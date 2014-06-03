Param([string]$version = $null)

$Archs = {"Portable-Net45+Win8+WP8+WPA81", "Net45", "WP8", "WP81", "Win8", "MonoAndroid", "MonoTouch", "Portable-Win81+WPA81"}

$Projects = {
    "ReactiveApp", "ReactiveApp.Munq", "ReactiveApp.Xaml", "ReactiveApp.Android", "ReactiveApp.iOS"
}

$MSBuildLocation = "C:\Program Files (x86)\MSBuild\12.0\bin"

$SlnFileExists = Test-Path ".\ReactiveApp.sln"
if ($SlnFileExists -eq $False) {
    echo "*** ERROR: Run this in the project root ***"
    exit -1
}


### Update NuGet
& ".\tools\NuGet.exe" update -self
& ".\tools\NuGet.exe" restore .\ReactiveApp.sln
& "$MSBuildLocation\MSBuild.exe" /t:Rebuild /p:Configuration=Release /p:Platform="Any CPU" /maxcpucount:1 .\ReactiveApp.sln

###
### Build the Release directory
###

if (Test-Path .\Release) {
    rmdir -r -force .\Release
}

if (!(Test-Path .\Releases)) {
    mkdir -Path ".\Releases"
}

foreach-object $Archs | %{mkdir -Path ".\Release\$_"}

foreach-object $Archs | %{
    $currentArch = $_
    
	foreach-object $Projects | %{cp -r -fo ".\src\$_\bin\Release\$currentArch\*" ".\Release\$currentArch"}
    foreach-object $Projects | %{cp -r -fo ".\src\$_.$currentArch\bin\Release\$currentArch\*" ".\Release\$currentArch"}
}


###
### Build NuGet Packages
###

if (Test-Path .\NuGet-Release) {
    rm -r -fo .\NuGet-Release
}

# Update Nuspecs if we have a version

cp -r .\NuGet .\NuGet-Release

$libDirs = ls -r .\NuGet-Release | ?{$_.Name -eq "lib"}
$srcDirs = ls -r .\NuGet-Release | ?{$_.Name -eq "src"}

foreach ($dir in $libDirs) {
	robocopy ".\Release" $dir.FullName /S /XL
}

# copy source
foreach ($dir in $srcDirs) {

	$subDirs = ls $dir.FullName | ?{$_.PsIsContainer}
	
	foreach ($subDir in $subDirs) {
		robocopy ".\src\$subDir" "$($dir.FullName)\$subDir" *.cs /S
	}
}

$stubs = ls -r -file .\NuGet-Release | ?{$_.Length -eq 0} | ?{!$_.FullName.Contains("src")}
if ($stubs) {
    echo "*** BUILD FAILED ***"
    echo ""
	echo $stubs
	echo ""
    echo "*** There are still stubs in the NuGet output, did you fully build? ***"
    exit 1
}

$specFiles = ls -r .\NuGet-Release | ?{$_.Name.EndsWith(".nuspec")}
$specFiles | %{
	$specFile = $_.FullName
    $content = Get-Content $specFile
    $newContent = $content -replace "__Version__", $Version
    Set-Content $specFile $newcontent
}
$specFiles | %{ & ".\tools\NuGet.exe" pack -symbols $_.FullName -OutputDirectory "Releases\"}

