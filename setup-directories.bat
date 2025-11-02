@echo off
echo Setting up Alert Scheduling System directories...

set BASE_DIR=C:\AlertFiles

echo Creating base directory: %BASE_DIR%
mkdir "%BASE_DIR%" 2>nul

echo Creating Pending directory: %BASE_DIR%\Pending
mkdir "%BASE_DIR%\Pending" 2>nul

echo Creating Treated directory: %BASE_DIR%\Treated
mkdir "%BASE_DIR%\Treated" 2>nul

echo Creating sample directory structure...
mkdir "%BASE_DIR%\Samples" 2>nul

echo Copying sample files...
copy "sample-excel-files\*.xlsx" "%BASE_DIR%\Samples\" 2>nul
copy "sample-data\*.csv" "%BASE_DIR%\Samples\" 2>nul

echo.
echo Directory structure created:
echo %BASE_DIR%\
echo ├── Pending\     (Place Excel files here for processing)
echo ├── Treated\     (Processed files moved here by date)
echo └── Samples\     (Sample files for reference)
echo.
echo Setup complete!

pause