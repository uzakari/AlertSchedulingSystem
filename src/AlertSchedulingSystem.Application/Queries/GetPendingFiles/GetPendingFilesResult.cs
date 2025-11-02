namespace AlertSchedulingSystem.Application.Queries.GetPendingFiles;

public record GetPendingFilesResult(IEnumerable<string> FilePaths);