using MediatR;

namespace AlertSchedulingSystem.Application.Queries.GetPendingFiles;

public record GetPendingFilesQuery(string DirectoryPath) : IRequest<GetPendingFilesResult>;