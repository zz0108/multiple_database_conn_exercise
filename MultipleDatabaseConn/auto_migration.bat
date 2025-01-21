@echo off
echo Starting database migration automation...

REM Step 1: Run PostgreSQL Docker container
echo Step 1: Running PostgreSQL Docker container...
call script\run_postgres_docker.bat
if errorlevel 1 (
    echo Error: Failed to run PostgreSQL Docker container
    echo Error code: %errorlevel%
    timeout /t 10
    pause
    exit /b 1
)
echo PostgreSQL Docker container started successfully.
timeout /t 3

REM Step 2: Add Migration using dotnet ef
echo Step 2: Adding Initial Migration...
dotnet ef migrations add InitialCreate --context MigrationDbContext
if errorlevel 1 (
    echo Error: Failed to add migration
    echo Error code: %errorlevel%
    timeout /t 10
    pause
    exit /b 1
)
echo Migration added successfully.
timeout /t 3

REM Step 3: Update Database
echo Step 3: Updating Database...
dotnet ef database update --context MigrationDbContext
if errorlevel 1 (
    echo Error: Failed to update database
    echo Error code: %errorlevel%
    timeout /t 10
    pause
    exit /b 1
)
echo Database updated successfully.

echo All steps completed successfully!
timeout /t 5
pause