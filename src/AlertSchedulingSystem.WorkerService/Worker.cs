using AlertSchedulingSystem.Application.Commands.ProcessAlertFile;
using AlertSchedulingSystem.Application.Queries.GetPendingFiles;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlertSchedulingSystem.WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly AlertProcessingOptions _options;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IOptions<AlertProcessingOptions> options)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Alert Scheduling System Worker Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingAlerts(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing alerts");
            }

            await Task.Delay(_options.ProcessingIntervalMs, stoppingToken);
        }

        _logger.LogInformation("Alert Scheduling System Worker Service stopped");
    }

    private async Task ProcessPendingAlerts(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var query = new GetPendingFilesQuery(_options.PendingFilesDirectory);
        var result = await mediator.Send(query, cancellationToken);

        foreach (var filePath in result.FilePaths)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            _logger.LogInformation("Processing file: {FilePath}", filePath);

            var command = new ProcessAlertFileCommand(filePath);
            var processResult = await mediator.Send(command, cancellationToken);

            if (processResult.Success)
            {
                _logger.LogInformation("Successfully processed {ProcessedAlerts} alerts from file: {FilePath}",
                    processResult.ProcessedAlerts, filePath);
            }
            else
            {
                _logger.LogError("Failed to process file {FilePath}: {ErrorMessage}",
                    filePath, processResult.ErrorMessage);
            }
        }
    }
}

public class AlertProcessingOptions
{
    public string PendingFilesDirectory { get; set; } = "/Users/umar/Documents/AlertSchedulingSystem/test-data/pending";
    public int ProcessingIntervalMs { get; set; } = 30000; // 30 seconds
}
