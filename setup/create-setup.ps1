# powershell.exe -ExecutionPolicy ByPass -File .\create-setup.ps1

# Release procedure
#
#- Update version in AssemblyInfo.cs file (use only major/minor)
#- Recompile with RELEASE configuration (via VS or use compile-release.bat)
#- Run create-setup.bat which will create executables in /dist/
#- Copy /dist/ into web server (manifest will point to the latest version)


Write-Host "Building setup file";

# grab folder path where this script is located
$rootPath = Split-Path -Parent $MyInvocation.MyCommand.Definition;

# other paths
$exeFile = Join-Path $rootPath -ChildPath "..\LightSqlProfiler\bin\Release\LightSqlProfiler.exe" -Resolve;
$issPath = Join-Path $rootPath -ChildPath "..\packages\Tools.InnoSetup.5.6.1\tools\ISCC.exe" -Resolve;
$cfgPath = Join-Path $rootPath -ChildPath ".\setup.iss" -Resolve;
$mnfPath = Join-Path $rootPath -ChildPath "..\dist\manifest.json";

# get version of final EXE file
$fullVersion = (Get-Item $exeFile).VersionInfo;
$version = "{0}.{1}" -f $fullVersion.FileMajorPart, $fullVersion.FileMinorPart;

Write-Host "Building setup file for version: $version";

$_ = & $issPath $cfgPath /Q /DMyAppVersion="$version";

# write manifest file
Write-Host "Writing manifest file: $mnfPath";
$json = @{
    LatestVersion = $version;
    Location = "http://namas.geciauskas.com/lsp/";
};

$json | ConvertTo-Json | Out-File -FilePath $mnfPath;

Write-Host "Done";
