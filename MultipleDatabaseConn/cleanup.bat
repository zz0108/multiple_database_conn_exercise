@echo off
echo Starting cleanup process...

REM Step 1: Remove Migrations folder
echo Step 1: Removing Migrations folder...
if exist "Migrations" (
    rd /s /q "Migrations"
    if errorlevel 1 (
        echo Error: Failed to remove Migrations folder
        echo Error code: %errorlevel%
        timeout /t 10
        pause
        exit /b 1
    )
    echo Migrations folder removed successfully.
) else (
    echo Migrations folder not found, skipping...
)
timeout /t 3

REM Step 2: Run Docker cleanup script
echo Step 2: Cleaning up Docker containers...
call script\cleanup_postgres_docker.bat
if errorlevel 1 (
    echo Error: Failed to cleanup Docker containers
    echo Error code: %errorlevel%
    timeout /t 10
    pause
    exit /b 1
)
echo Docker cleanup completed successfully.

echo All cleanup steps completed successfully!
timeout /t 5
pause