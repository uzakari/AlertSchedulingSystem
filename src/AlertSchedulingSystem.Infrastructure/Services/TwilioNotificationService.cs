using AlertSchedulingSystem.Domain.Entities;
using AlertSchedulingSystem.Domain.Interfaces;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace AlertSchedulingSystem.Infrastructure.Services;

public class TwilioNotificationService : INotificationService
{
    private readonly TwilioOptions _options;

    public TwilioNotificationService(IOptions<TwilioOptions> options)
    {
        _options = options.Value;
        TwilioClient.Init(_options.AccountSid, _options.AuthToken);
    }

    public async Task SendAlertAsync(Alert alert)
    {
        if (IsEmailAddress(alert.Recipient))
        {
            await SendEmailAsync(alert);
        }
        else if (IsPhoneNumber(alert.Recipient))
        {
            await SendSmsAsync(alert);
        }
        else
        {
            throw new ArgumentException($"Invalid recipient format: {alert.Recipient}");
        }
    }

    private async Task SendEmailAsync(Alert alert)
    {
        var message = await MessageResource.CreateAsync(
            body: $"Subject: {alert.Subject}\n\n{alert.Message}",
            from: new PhoneNumber(_options.FromPhoneNumber),
            to: new PhoneNumber(_options.ToPhoneNumber)
        );
    }

    private async Task SendSmsAsync(Alert alert)
    {
        var message = await MessageResource.CreateAsync(
            body: $"{alert.Subject}\n\n{alert.Message}",
            from: new PhoneNumber(_options.FromPhoneNumber),
            to: new PhoneNumber(alert.Recipient)
        );
    }

    private static bool IsEmailAddress(string input)
    {
        return input.Contains('@') && input.Contains('.');
    }

    private static bool IsPhoneNumber(string input)
    {
        return input.All(c => char.IsDigit(c) || c == '+' || c == '-' || c == '(' || c == ')' || c == ' ');
    }
}

public class TwilioOptions
{
    public string AccountSid { get; set; } = string.Empty;
    public string AuthToken { get; set; } = string.Empty;
    public string FromPhoneNumber { get; set; } = string.Empty;
    public string ToPhoneNumber { get; set; } = string.Empty;
}