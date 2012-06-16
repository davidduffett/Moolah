@echo off

if '%1' == '/?' goto usage
if '%1' == '-?' goto usage
if '%1' == '?' goto usage
if '%1' == '/help' goto usage
if '%1' == '--help' goto usage
if '%1' == '-help' goto usage

SET NANT="%~dp0..\tools\nant\nant.exe"
SET BUILD_FILE="%~dp0main.build"

%NANT% /f:%BUILD_FILE% %*

if %ERRORLEVEL% NEQ 0 goto errors

goto finish

:usage
echo.
echo Usage: build.bat
echo.
goto finish

:errors
EXIT /B %ERRORLEVEL%

:finish