using AlertSchedulingSystem.Domain.Entities;
using AlertSchedulingSystem.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AlertSchedulingSystem.Application.Commands.ProcessAlertFile;

public class ProcessAlertFileCommandHandler : IRequestHandler<ProcessAlertFileCommand, ProcessAlertFileResult>
{
    private readonly IExcelReader _excelReader;
    private readonly INotificationService _notificationService;
    private readonly ITemplateService _templateService;
    private readonly IFileManager _fileManager;
    private readonly ILogger<ProcessAlertFileCommandHandler> _logger;

    public ProcessAlertFileCommandHandler(
        IExcelReader excelReader,
        INotificationService notificationService,
        ITemplateService templateService,
        IFileManager fileManager,
        ILogger<ProcessAlertFileCommandHandler> logger)
    {
        _excelReader = excelReader;
        _notificationService = notificationService;
        _templateService = templateService;
        _fileManager = fileManager;
        _logger = logger;
    }

    public async Task<ProcessAlertFileResult> Handle(ProcessAlertFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Processing alert file: {FilePath}", request.FilePath);

            var alerts = await _excelReader.ReadAlertsFromFileAsync(request.FilePath);
            var processedCount = 0;

            foreach (var alert in alerts)
            {
                try
                {
                    alert.Status = AlertStatus.Processing;

                    var template = await _templateService.GetTemplateByTypeAsync(alert.AlertType);
                    if (template == null)
                    {
                        _logger.LogWarning("No template found for alert type: {AlertType}", alert.AlertType);
                        alert.Status = AlertStatus.Failed;
                        continue;
                    }

                    alert.Subject = await _templateService.RenderTemplateAsync(template.SubjectTemplate, alert.TemplateData);
                    alert.Message = await _templateService.RenderTemplateAsync(template.MessageTemplate, alert.TemplateData);

                    await _notificationService.SendAlertAsync(alert);

                    alert.Status = AlertStatus.Sent;
                    processedCount++;

                    _logger.LogInformation("Alert sent successfully to {Recipient}", alert.Recipient);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process alert for {Recipient}", alert.Recipient);
                    alert.Status = AlertStatus.Failed;
                }
            }

            var treatedFolderPath = Path.Combine(Path.GetDirectoryName(request.FilePath)!, "treated", DateTime.Now.ToString("yyyy-MM-dd"));
            await _fileManager.EnsureDirectoryExistsAsync(treatedFolderPath);
            await _fileManager.MoveFileToTreatedFolderAsync(request.FilePath, treatedFolderPath);

            _logger.LogInformation("Processed {ProcessedCount} alerts from file: {FilePath}", processedCount, request.FilePath);

            return new ProcessAlertFileResult(true, processedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process alert file: {FilePath}", request.FilePath);
            return new ProcessAlertFileResult(false, 0, ex.Message);
        }
    }
}