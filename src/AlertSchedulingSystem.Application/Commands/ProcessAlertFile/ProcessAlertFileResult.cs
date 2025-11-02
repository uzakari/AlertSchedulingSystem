namespace AlertSchedulingSystem.Application.Commands.ProcessAlertFile;

public record ProcessAlertFileResult(bool Success, int ProcessedAlerts, string? ErrorMessage = null);