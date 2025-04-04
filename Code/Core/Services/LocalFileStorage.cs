using Core.Interfaces;

namespace Core.Services;

public class LocalFileStorage(IWebHostEnvironment env, IFileOperations fileOperations) : IFileStorage
{
    private readonly IFileOperations _fileOperations = fileOperations;
    public async Task StoreFile(IFormFile file, string key)
    {
        const string rootPath = "PhotoStore";

        var wwwRootFolder = env.WebRootPath;
        var userFolder = _fileOperations.Combine(wwwRootFolder, rootPath); // Path.Combine(wwwRootFolder, rootPath);
        var folder = _fileOperations.Combine(userFolder, key); //Path.Combine(userFolder, key);

        // if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        if (!_fileOperations.DirectoryExists(folder)) _fileOperations.CreateDirectory(folder);

        //var fullFilePath = Path.Combine(folder, Path.GetFileName(file.FileName) ?? string.Empty);

        var fullFilePath = _fileOperations.Combine(folder, _fileOperations.GetFileName(file.FileName) ?? string.Empty);

        //await using var targetStream = new FileStream(fullFilePath, FileMode.Create);

        await using var targetStream = _fileOperations.CreateFileStream(fullFilePath);
        await file.CopyToAsync(targetStream);
        targetStream.Close();
    }
}