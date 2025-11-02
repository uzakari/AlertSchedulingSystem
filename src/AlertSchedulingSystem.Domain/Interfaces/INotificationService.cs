using AlertSchedulingSystem.Domain.Entities;

namespace AlertSchedulingSystem.Domain.Interfaces;

public interface INotificationService
{
    Task SendAlertAsync(Alert alert);
}