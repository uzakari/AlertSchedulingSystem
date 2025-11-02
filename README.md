# Alert Scheduling System

A Windows Service application that monitors Excel files containing alert information and sends customized notifications using Twilio based on alert templates.

## Architecture

This solution follows Clean Architecture principles with CQRS pattern:

- **Domain Layer**: Contains entities, interfaces, and business rules
- **Application Layer**: Implements CQRS commands/queries and handlers
- **Infrastructure Layer**: Contains external service implementations (Excel reading, Twilio integration, file management)
- **Worker Service**: Background service for processing alerts

## Features

- Monitors a directory for pending Excel files containing alert data
- Reads alert type and recipient information from Excel files
- Applies customizable message templates based on alert type
- Sends notifications via Twilio (SMS/Email)
- Moves processed files to a daily treated folder
- Can be installed as a Windows Service
- Configurable processing intervals

## Setup Instructions

### 1. Configuration

Edit `appsettings.json` in the WorkerService project:

```json
{
  "AlertProcessing": {
    "PendingFilesDirectory": "C:\\AlertFiles\\Pending",
    "ProcessingIntervalMs": 30000
  },
  "Twilio": {
    "AccountSid": "YOUR_TWILIO_ACCOUNT_SID",
    "AuthToken": "YOUR_TWILIO_AUTH_TOKEN",
    "FromPhoneNumber": "YOUR_TWILIO_PHONE_NUMBER",
    "ToPhoneNumber": "DEFAULT_RECIPIENT_PHONE_NUMBER"
  }
}
```

### 2. Excel File Format

Excel files should have the following structure:

| AlertType | Recipient | Title | Description | Timestamp | Location |
|-----------|-----------|--------|-------------|-----------|----------|
| critical  | +1234567890 | System Down | Server failure | 2025-01-01 10:00 | DataCenter1 |
| warning   | user@email.com | High CPU | CPU usage 90% | 2025-01-01 10:30 | Server01 |

- **Column 1**: AlertType (critical, warning, info, maintenance)
- **Column 2**: Recipient (phone number or email)
- **Columns 3+**: Template data fields

### 3. Alert Templates

The system includes predefined templates:

- **Critical**: For urgent system alerts
- **Warning**: For warning notifications
- **Info**: For informational messages
- **Maintenance**: For scheduled maintenance notifications

### 4. Building and Installing

```bash
# Build the solution
dotnet build

# Publish for Windows Service deployment
dotnet publish src/AlertSchedulingSystem.WorkerService -c Release -o publish

# Install as Windows Service (run as Administrator)
install-service.bat

# Uninstall Windows Service (run as Administrator)
uninstall-service.bat
```

### 5. Directory Structure

```
C:\AlertFiles\
├── Pending\          # Place Excel files here for processing
└── Treated\          # Processed files are moved here
    └── 2025-01-01\   # Daily folders for organization
```

## Development

### Prerequisites

- .NET 10.0 SDK
- Twilio Account (for notifications)
- Visual Studio 2022 or VS Code

### Project Structure

```
AlertSchedulingSystem/
├── src/
│   ├── AlertSchedulingSystem.Domain/
│   ├── AlertSchedulingSystem.Application/
│   ├── AlertSchedulingSystem.Infrastructure/
│   └── AlertSchedulingSystem.WorkerService/
├── install-service.bat
├── uninstall-service.bat
└── README.md
```

### Key Components

- **Alert Entity**: Represents an alert with type, recipient, and template data
- **CQRS Commands**: ProcessAlertFileCommand for processing files
- **CQRS Queries**: GetPendingFilesQuery for finding files to process
- **Services**: ExcelReader, NotificationService, TemplateService, FileManager
- **Worker**: Background service that orchestrates the alert processing

## Troubleshooting

1. **Service won't start**: Check event logs for detailed error messages
2. **Files not processing**: Verify directory permissions and Excel file format
3. **Notifications not sending**: Validate Twilio configuration and credentials
4. **Template errors**: Ensure Excel files contain required template data fields

## License

This project is for internal use and follows company security policies.# AlertSchedulingSystem
