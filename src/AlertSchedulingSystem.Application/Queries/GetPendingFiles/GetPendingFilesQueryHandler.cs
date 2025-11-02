using AlertSchedulingSystem.Domain.Interfaces;
using MediatR;

namespace AlertSchedulingSystem.Application.Queries.GetPendingFiles;

public class GetPendingFilesQueryHandler : IRequestHandler<GetPendingFilesQuery, GetPendingFilesResult>
{
    private readonly IFileManager _fileManager;

    public GetPendingFilesQueryHandler(IFileManager fileManager)
    {
        _fileManager = fileManager;
    }

    public async Task<GetPendingFilesResult> Handle(GetPendingFilesQuery request, CancellationToken cancellationToken)
    {
        var filePaths = await _fileManager.GetPendingFilesAsync(request.DirectoryPath);
        return new GetPendingFilesResult(filePaths);
    }
}