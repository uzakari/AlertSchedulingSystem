@echo off
echo Uninstalling Alert Scheduling System Windows Service...

echo Stopping service...
sc stop "AlertSchedulingSystem"

echo Deleting service...
sc delete "AlertSchedulingSystem"
if %errorlevel% == 0 (
    echo Service uninstalled successfully.
) else (
    echo Failed to uninstall service.
)

pause