using AlertSchedulingSystem.WorkerService;
using AlertSchedulingSystem.Domain.Interfaces;
using AlertSchedulingSystem.Infrastructure.Services;
using AlertSchedulingSystem.Application.Commands.ProcessAlertFile;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<AlertProcessingOptions>(
    builder.Configuration.GetSection("AlertProcessing"));

builder.Services.Configure<TwilioOptions>(
    builder.Configuration.GetSection("Twilio"));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(ProcessAlertFileCommand).Assembly);
});

builder.Services.AddScoped<IExcelReader, ExcelReaderService>();
builder.Services.AddScoped<INotificationService, TwilioNotificationService>();
builder.Services.AddScoped<ITemplateService, TemplateService>();
builder.Services.AddScoped<IFileManager, FileManagerService>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
