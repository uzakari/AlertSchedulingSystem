using AlertSchedulingSystem.Domain.Entities;
using AlertSchedulingSystem.Domain.Interfaces;
using ClosedXML.Excel;

namespace AlertSchedulingSystem.Infrastructure.Services;

public class ExcelReaderService : IExcelReader
{
    public Task<IEnumerable<Alert>> ReadAlertsFromFileAsync(string filePath)
    {
        var alerts = new List<Alert>();

        using var workbook = new XLWorkbook(filePath);
        var worksheet = workbook.Worksheets.FirstOrDefault();

        if (worksheet == null)
            return Task.FromResult<IEnumerable<Alert>>(alerts);

        var lastRowUsed = worksheet.LastRowUsed()?.RowNumber() ?? 0;

        for (int row = 2; row <= lastRowUsed; row++)
        {
            var alertType = worksheet.Cell(row, 1).Value.ToString();
            var recipient = worksheet.Cell(row, 2).Value.ToString();

            if (string.IsNullOrEmpty(alertType) || string.IsNullOrEmpty(recipient))
                continue;

            var alert = new Alert
            {
                Id = Guid.NewGuid(),
                AlertType = alertType,
                Recipient = recipient,
                CreatedAt = DateTime.UtcNow,
                Status = AlertStatus.Pending,
                SourceFilePath = filePath,
                TemplateData = ReadTemplateData(worksheet, row)
            };

            alerts.Add(alert);
        }

        return Task.FromResult<IEnumerable<Alert>>(alerts);
    }

    private Dictionary<string, object> ReadTemplateData(IXLWorksheet worksheet, int row)
    {
        var data = new Dictionary<string, object>();

        var lastColumnUsed = worksheet.LastColumnUsed()?.ColumnNumber() ?? 0;

        for (int col = 3; col <= lastColumnUsed; col++)
        {
            var header = worksheet.Cell(1, col).Value.ToString();
            var value = worksheet.Cell(row, col).Value.ToString();

            if (!string.IsNullOrEmpty(header) && !string.IsNullOrEmpty(value))
            {
                data[header] = value;
            }
        }

        return data;
    }
}