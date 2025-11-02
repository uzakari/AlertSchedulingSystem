namespace AlertSchedulingSystem.Domain.Entities;

public class AlertTemplate
{
    public string AlertType { get; set; } = string.Empty;
    public string SubjectTemplate { get; set; } = string.Empty;
    public string MessageTemplate { get; set; } = string.Empty;
    public List<string> RequiredFields { get; set; } = new();
}