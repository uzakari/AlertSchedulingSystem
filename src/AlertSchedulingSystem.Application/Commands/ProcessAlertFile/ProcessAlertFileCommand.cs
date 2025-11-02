using MediatR;

namespace AlertSchedulingSystem.Application.Commands.ProcessAlertFile;

public record ProcessAlertFileCommand(string FilePath) : IRequest<ProcessAlertFileResult>;