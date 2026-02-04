@echo off
REM Clean install for CI: remove node_modules then npm install.
REM Fixes "Invalid package name __ngcc_entry_points__.json" when npm reuses a broken node_modules.
setlocal
cd /d "%~dp0.."
if exist node_modules (
  rmdir /s /q node_modules
)
call npm install
exit /b %ERRORLEVEL%
