namespace AlertSchedulingSystem.Domain.Entities;

public class Alert
{
    public Guid Id { get; set; }
    public string AlertType { get; set; } = string.Empty;
    public string Recipient { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public AlertStatus Status { get; set; }
    public string SourceFilePath { get; set; } = string.Empty;
    public Dictionary<string, object> TemplateData { get; set; } = new();
}

public enum AlertStatus
{
    Pending,
    Processing,
    Sent,
    Failed
}