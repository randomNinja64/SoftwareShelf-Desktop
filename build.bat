@echo off
setlocal EnableExtensions
set ROOT=%~dp0
set OUT=%ROOT%SoftwareShelf Desktop
set SETUP=%ROOT%SoftwareShelf Desktop Setup.exe
set NSI=%ROOT%InstallScript.nsi

:: Find MSBuild
for /f "usebackq delims=" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do set MSBUILD=%%i
if not defined MSBUILD (echo ERROR: MSBuild not found. & exit /b 1)

:: Build solution
echo Building solution ...
"%MSBUILD%" "%ROOT%SoftwareShelf Desktop.sln" /p:Configuration=Release /m /v:minimal || exit /b %ERRORLEVEL%
echo.

:: Stage output
if exist "%OUT%" rmdir /s /q "%OUT%"
mkdir "%OUT%"

copy /Y "%ROOT%bin\Release\SoftwareShelf Desktop.exe" "%OUT%\" >nul || exit /b 1
copy /Y "%ROOT%bin\Release\SoftwareShelf Desktop.exe.config" "%OUT%\" >nul || exit /b 1
if not exist "%ROOT%bin\Release\Newtonsoft.Json.dll" (
  echo ERROR: staged build missing Newtonsoft.Json.dll
  exit /b 1
)
copy /Y "%ROOT%bin\Release\Newtonsoft.Json.dll" "%OUT%\" >nul

:: Optional deps from deps\ (aria2c for multithreaded downloads on XP and later)
echo Packaging optional dependencies ...
call :CopyIfExists "%ROOT%deps\aria2c.exe" "%OUT%\aria2c.exe"

:: Optional third-party license texts
call :CopyLicenses

:: Optional NSIS installer
call :BuildInstaller
if errorlevel 1 exit /b %ERRORLEVEL%

echo.
echo Done: %OUT%
if exist "%SETUP%" echo Installer: %SETUP%
exit /b 0

:CopyIfExists
if exist "%~1" (
  copy /Y "%~1" "%~2" >nul
  echo   packaged %~nx2
)
exit /b 0

:CopyLicenses
if not exist "%ROOT%THIRD_PARTY_LICENSES" exit /b 0
set "_any="
for %%F in ("%ROOT%THIRD_PARTY_LICENSES\*") do (
  if /I not "%%~nxF"==".gitkeep" set "_any=1"
)
if not defined _any exit /b 0
mkdir "%OUT%\THIRD_PARTY_LICENSES" 2>nul
for %%F in ("%ROOT%THIRD_PARTY_LICENSES\*") do (
  if /I not "%%~nxF"==".gitkeep" (
    copy /Y "%%F" "%OUT%\THIRD_PARTY_LICENSES\" >nul
    echo   packaged THIRD_PARTY_LICENSES\%%~nxF
  )
)
exit /b 0

:BuildInstaller
set "MAKENSIS="
:: NSIS 2.46 is the last release whose installer runs on Windows 98.
if exist "%ProgramFiles(x86)%\NSIS-2.46\makensis.exe" set "MAKENSIS=%ProgramFiles(x86)%\NSIS-2.46\makensis.exe"
if not defined MAKENSIS if exist "%ProgramFiles%\NSIS-2.46\makensis.exe" set "MAKENSIS=%ProgramFiles%\NSIS-2.46\makensis.exe"
if not defined MAKENSIS if exist "%ProgramFiles(x86)%\NSIS\makensis.exe" set "MAKENSIS=%ProgramFiles(x86)%\NSIS\makensis.exe"
if not defined MAKENSIS if exist "%ProgramFiles%\NSIS\makensis.exe" set "MAKENSIS=%ProgramFiles%\NSIS\makensis.exe"
where makensis >nul 2>&1 && for /f "delims=" %%i in ('where makensis') do if not defined MAKENSIS set "MAKENSIS=%%i"
if not defined MAKENSIS (
  echo NSIS not found; skipping installer.
  exit /b 0
)
"%MAKENSIS%" /VERSION | findstr /B /C:"v2." >nul
if errorlevel 1 echo WARNING: This NSIS is newer than 2.46. The installer will not run on Windows 98. Install NSIS 2.46 to "%ProgramFiles(x86)%\NSIS-2.46".
if not exist "%NSI%" (
  echo ERROR: NSIS script not found: %NSI%
  exit /b 1
)
if not exist "%OUT%\SoftwareShelf Desktop.exe" (
  echo ERROR: staged build missing SoftwareShelf Desktop.exe
  exit /b 1
)
if not exist "%OUT%\Newtonsoft.Json.dll" (
  echo ERROR: staged build missing Newtonsoft.Json.dll
  exit /b 1
)
echo.
echo Building installer ...
set "NSIDEFINES="
if exist "%OUT%\aria2c.exe" set "NSIDEFINES=%NSIDEFINES% /DHAVE_ARIA2"
if exist "%OUT%\THIRD_PARTY_LICENSES\" set "NSIDEFINES=%NSIDEFINES% /DHAVE_LICENSES"
"%MAKENSIS%" /V2 %NSIDEFINES% "/DDIST_DIR=%OUT%" "/DSETUP_OUT=%SETUP%" "%NSI%"
if errorlevel 1 (
  echo ERROR: NSIS installer build failed.
  exit /b 1
)
echo   packaged SoftwareShelf Desktop Setup.exe
exit /b 0
