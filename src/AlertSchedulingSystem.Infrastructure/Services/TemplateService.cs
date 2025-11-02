using AlertSchedulingSystem.Domain.Entities;
using AlertSchedulingSystem.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace AlertSchedulingSystem.Infrastructure.Services;

public class TemplateService : ITemplateService
{
    private readonly Dictionary<string, AlertTemplate> _templates;

    public TemplateService()
    {
        _templates = InitializeDefaultTemplates();
    }

    public async Task<AlertTemplate?> GetTemplateByTypeAsync(string alertType)
    {
        _templates.TryGetValue(alertType.ToLowerInvariant(), out var template);
        return await Task.FromResult(template);
    }

    public async Task<string> RenderTemplateAsync(string template, Dictionary<string, object> data)
    {
        var result = template;

        var regex = new Regex(@"\{\{(\w+)\}\}");
        var matches = regex.Matches(template);

        foreach (Match match in matches)
        {
            var placeholder = match.Value;
            var key = match.Groups[1].Value;

            if (data.TryGetValue(key, out var value))
            {
                result = result.Replace(placeholder, value?.ToString() ?? string.Empty);
            }
        }

        return await Task.FromResult(result);
    }

    private Dictionary<string, AlertTemplate> InitializeDefaultTemplates()
    {
        return new Dictionary<string, AlertTemplate>
        {
            ["critical"] = new AlertTemplate
            {
                AlertType = "critical",
                SubjectTemplate = "CRITICAL ALERT: {{Title}}",
                MessageTemplate = "This is a critical alert notification.\n\nDetails:\n{{Description}}\n\nTime: {{Timestamp}}\nLocation: {{Location}}",
                RequiredFields = new List<string> { "Title", "Description", "Timestamp", "Location" }
            },
            ["warning"] = new AlertTemplate
            {
                AlertType = "warning",
                SubjectTemplate = "Warning: {{Title}}",
                MessageTemplate = "A warning has been issued.\n\nDetails:\n{{Description}}\n\nTime: {{Timestamp}}",
                RequiredFields = new List<string> { "Title", "Description", "Timestamp" }
            },
            ["info"] = new AlertTemplate
            {
                AlertType = "info",
                SubjectTemplate = "Information: {{Title}}",
                MessageTemplate = "Information notification:\n\n{{Description}}\n\nFor more details, please contact support.",
                RequiredFields = new List<string> { "Title", "Description" }
            },
            ["maintenance"] = new AlertTemplate
            {
                AlertType = "maintenance",
                SubjectTemplate = "Scheduled Maintenance: {{Title}}",
                MessageTemplate = "Scheduled maintenance notification:\n\n{{Description}}\n\nStart Time: {{StartTime}}\nEnd Time: {{EndTime}}\n\nWe apologize for any inconvenience.",
                RequiredFields = new List<string> { "Title", "Description", "StartTime", "EndTime" }
            },
            ["birthday"] = new AlertTemplate
            {
                AlertType = "birthday",
                SubjectTemplate = "🎉 Happy Birthday {{EmployeeName}}!",
                MessageTemplate = "Dear {{EmployeeName}},\n\nWishing you a very Happy Birthday! 🎂\n\nMay your special day be filled with happiness, joy, and wonderful memories.\n\nDepartment: {{Department}}\nDate of Birth: {{DateOfBirth}}\n\nBest wishes from the entire team!",
                RequiredFields = new List<string> { "EmployeeName", "Department", "DateOfBirth" }
            },
            ["anniversary"] = new AlertTemplate
            {
                AlertType = "anniversary",
                SubjectTemplate = "🎊 Work Anniversary - {{EmployeeName}}!",
                MessageTemplate = "Dear {{EmployeeName}},\n\nCongratulations on your work anniversary! 🎉\n\nToday marks {{YearsOfService}} year(s) of your valuable contribution to our organization.\n\nEmployee Details:\n- Name: {{EmployeeName}}\n- Department: {{Department}}\n- Start Date: {{StartDate}}\n- Years of Service: {{YearsOfService}}\n\nThank you for your dedication and hard work. Here's to many more successful years together!\n\nWith appreciation,\nHR Team",
                RequiredFields = new List<string> { "EmployeeName", "Department", "StartDate", "YearsOfService" }
            }
        };
    }
}