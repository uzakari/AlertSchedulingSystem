using OfficeOpenXml;
using System.IO;

namespace AlertSchedulingSystem.SampleData;

public class SampleAlertsGenerator
{
    public static void GenerateSampleExcelFile(string filePath)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Alerts");

        // Headers
        worksheet.Cells[1, 1].Value = "AlertType";
        worksheet.Cells[1, 2].Value = "Recipient";
        worksheet.Cells[1, 3].Value = "Title";
        worksheet.Cells[1, 4].Value = "Description";
        worksheet.Cells[1, 5].Value = "Timestamp";
        worksheet.Cells[1, 6].Value = "Location";
        worksheet.Cells[1, 7].Value = "StartTime";
        worksheet.Cells[1, 8].Value = "EndTime";

        // Sample data
        var sampleData = new[]
        {
            new { AlertType = "critical", Recipient = "+1234567890", Title = "Database Server Down", Description = "Primary database server is not responding", Timestamp = "2025-01-15 14:30:00", Location = "DataCenter-East", StartTime = "", EndTime = "" },
            new { AlertType = "warning", Recipient = "admin@company.com", Title = "High CPU Usage", Description = "Server CPU usage has exceeded 85% for 15 minutes", Timestamp = "2025-01-15 14:45:00", Location = "Server-Web01", StartTime = "", EndTime = "" },
            new { AlertType = "info", Recipient = "+1987654321", Title = "Backup Completed", Description = "Daily backup process completed successfully", Timestamp = "2025-01-15 02:00:00", Location = "Backup-Server", StartTime = "", EndTime = "" },
            new { AlertType = "maintenance", Recipient = "users@company.com", Title = "Scheduled Maintenance", Description = "System will be offline for routine maintenance", Timestamp = "2025-01-16 08:00:00", Location = "All Systems", StartTime = "2025-01-16 02:00:00", EndTime = "2025-01-16 06:00:00" },
            new { AlertType = "critical", Recipient = "emergency@company.com", Title = "Security Breach", Description = "Unauthorized access attempt detected", Timestamp = "2025-01-15 16:20:00", Location = "Firewall-DMZ", StartTime = "", EndTime = "" },
            new { AlertType = "warning", Recipient = "+1555123456", Title = "Disk Space Low", Description = "Disk usage on /var partition is at 90%", Timestamp = "2025-01-15 17:00:00", Location = "File-Server", StartTime = "", EndTime = "" }
        };

        // Populate data rows
        for (int i = 0; i < sampleData.Length; i++)
        {
            var row = i + 2;
            var data = sampleData[i];

            worksheet.Cells[row, 1].Value = data.AlertType;
            worksheet.Cells[row, 2].Value = data.Recipient;
            worksheet.Cells[row, 3].Value = data.Title;
            worksheet.Cells[row, 4].Value = data.Description;
            worksheet.Cells[row, 5].Value = data.Timestamp;
            worksheet.Cells[row, 6].Value = data.Location;
            worksheet.Cells[row, 7].Value = data.StartTime;
            worksheet.Cells[row, 8].Value = data.EndTime;
        }

        // Auto-fit columns
        worksheet.Cells.AutoFitColumns();

        // Save the file
        var fileInfo = new FileInfo(filePath);
        package.SaveAs(fileInfo);
    }

    public static void Main(string[] args)
    {
        var outputDir = @"C:\AlertFiles\Pending";
        Directory.CreateDirectory(outputDir);

        var fileName = $"sample_alerts_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
        var filePath = Path.Combine(outputDir, fileName);

        GenerateSampleExcelFile(filePath);

        Console.WriteLine($"Sample Excel file generated: {filePath}");
        Console.WriteLine("\nFile contains the following alert types:");
        Console.WriteLine("- Critical: Database server down, Security breach");
        Console.WriteLine("- Warning: High CPU usage, Disk space low");
        Console.WriteLine("- Info: Backup completed");
        Console.WriteLine("- Maintenance: Scheduled maintenance");
    }
}