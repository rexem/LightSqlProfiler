; #define MyAppVersion "0.3" ; dynamically set via command line

#define MyAppName "Light SQL Profiler"
#define MyAppPublisher "Ramunas Geciauskas"
#define MyAppURL "http://namas.geciauskas.com"
#define MyAppExeName "LightSqlProfiler.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{A6296073-948D-4B0C-B33F-A894C6AAA977}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DisableProgramGroupPage=yes
LicenseFile=.\..\license.txt
OutputDir=.\..\dist
OutputBaseFilename=LightSqlProfilerSetup-{#MyAppVersion}
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: ".\..\LightSqlProfiler\bin\Release\LightSqlProfiler.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\..\LightSqlProfiler\bin\Release\LightSqlProfiler.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\..\LightSqlProfiler\bin\Release\ControlzEx.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\..\LightSqlProfiler\bin\Release\ICSharpCode.AvalonEdit.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\..\LightSqlProfiler\bin\Release\log4net.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\..\LightSqlProfiler\bin\Release\MahApps.Metro.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\..\LightSqlProfiler\bin\Release\MahApps.Metro.IconPacks.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\..\LightSqlProfiler\bin\Release\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\..\LightSqlProfiler\bin\Release\System.Windows.Interactivity.dll"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{commonprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

