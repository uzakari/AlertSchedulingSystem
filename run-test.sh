#!/bin/bash

echo "🚀 Starting Alert Scheduling System Test..."
echo ""

# Navigate to WorkerService directory
cd src/AlertSchedulingSystem.WorkerService

echo "📋 Current Configuration:"
echo "- Environment: Development"
echo "- Pending Directory: /Users/umar/Documents/AlertSchedulingSystem/test-data/pending"
echo "- Processing Interval: 10 seconds"
echo "- Log Level: Debug"
echo ""

echo "📁 Files in pending directory:"
ls -la ../../test-data/pending/
echo ""

echo "🔧 Building application..."
dotnet build

echo ""
echo "▶️  Starting application (Press Ctrl+C to stop)..."
echo "Watch the logs to see HR notifications being processed..."
echo ""

# Run the application
dotnet run --environment Development