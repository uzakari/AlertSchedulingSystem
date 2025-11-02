namespace AlertSchedulingSystem.Domain.Interfaces;

public interface IFileManager
{
    Task<IEnumerable<string>> GetPendingFilesAsync(string directoryPath);
    Task MoveFileToTreatedFolderAsync(string sourceFilePath, string treatedFolderPath);
    Task EnsureDirectoryExistsAsync(string directoryPath);
}