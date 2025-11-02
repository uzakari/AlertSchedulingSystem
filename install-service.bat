@echo off
echo Installing Alert Scheduling System as Windows Service...

sc create "AlertSchedulingSystem" binPath= "%~dp0AlertSchedulingSystem.WorkerService.exe" start= auto
if %errorlevel% == 0 (
    echo Service installed successfully.
    echo Starting service...
    sc start "AlertSchedulingSystem"
    if %errorlevel% == 0 (
        echo Service started successfully.
    ) else (
        echo Failed to start service.
    )
) else (
    echo Failed to install service.
)

pause