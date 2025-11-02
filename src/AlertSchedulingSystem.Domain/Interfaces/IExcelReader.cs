using AlertSchedulingSystem.Domain.Entities;

namespace AlertSchedulingSystem.Domain.Interfaces;

public interface IExcelReader
{
    Task<IEnumerable<Alert>> ReadAlertsFromFileAsync(string filePath);
}