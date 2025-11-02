using AlertSchedulingSystem.Domain.Entities;

namespace AlertSchedulingSystem.Domain.Interfaces;

public interface ITemplateService
{
    Task<AlertTemplate?> GetTemplateByTypeAsync(string alertType);
    Task<string> RenderTemplateAsync(string template, Dictionary<string, object> data);
}