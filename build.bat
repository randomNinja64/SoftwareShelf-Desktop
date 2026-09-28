@echo off
setlocal EnableExtensions
set ROOT=%~dp0
set OUT=%ROOT%SoftwareShelf Desktop

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

echo.
echo Done: %OUT%
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
