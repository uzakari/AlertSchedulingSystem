using AlertSchedulingSystem.Domain.Interfaces;

namespace AlertSchedulingSystem.Infrastructure.Services;

public class FileManagerService : IFileManager
{
    public async Task<IEnumerable<string>> GetPendingFilesAsync(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
            return Enumerable.Empty<string>();

        var files = Directory.GetFiles(directoryPath, "*.xlsx")
            .Concat(Directory.GetFiles(directoryPath, "*.xls"))
            .OrderBy(f => File.GetCreationTime(f))
            .ToList();

        return await Task.FromResult(files);
    }

    public async Task MoveFileToTreatedFolderAsync(string sourceFilePath, string treatedFolderPath)
    {
        await EnsureDirectoryExistsAsync(treatedFolderPath);

        var fileName = Path.GetFileName(sourceFilePath);
        var destinationPath = Path.Combine(treatedFolderPath, fileName);

        if (File.Exists(destinationPath))
        {
            var timestamp = DateTime.Now.ToString("HHmmss");
            var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var extension = Path.GetExtension(fileName);
            destinationPath = Path.Combine(treatedFolderPath, $"{nameWithoutExtension}_{timestamp}{extension}");
        }

        File.Move(sourceFilePath, destinationPath);
    }

    public async Task EnsureDirectoryExistsAsync(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        await Task.CompletedTask;
    }
}