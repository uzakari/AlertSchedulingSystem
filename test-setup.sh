#!/bin/bash

echo "Setting up Alert Scheduling System for testing..."

# Create test directories
CURRENT_DIR=$(pwd)
TEST_PENDING_DIR="$CURRENT_DIR/test-data/pending"
TEST_TREATED_DIR="$CURRENT_DIR/test-data/treated"

echo "Creating test directories..."
mkdir -p "$TEST_PENDING_DIR"
mkdir -p "$TEST_TREATED_DIR"

echo "Copying sample files to pending directory..."
cp sample-excel-files/*.xlsx "$TEST_PENDING_DIR/"

echo "Test setup complete!"
echo ""
echo "Directory structure:"
echo "├── test-data/"
echo "│   ├── pending/     (Excel files will be processed from here)"
echo "│   └── treated/     (Processed files will be moved here)"
echo ""
echo "To test the application:"
echo "1. cd src/AlertSchedulingSystem.WorkerService"
echo "2. dotnet run --environment Development"
echo ""
echo "The application will:"
echo "- Monitor: $TEST_PENDING_DIR"
echo "- Process files every 10 seconds"
echo "- Move processed files to: $TEST_TREATED_DIR"
echo "- Log detailed information for debugging"