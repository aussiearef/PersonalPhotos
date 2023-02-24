using System.IO;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Core.Services;

public class LocalFileStorage : IFileStorage
{
    private readonly IHostingEnvironment _env;

    public LocalFileStorage(IHostingEnvironment env)
    {
        _env = env;
    }

    public async Task StoreFile(IFormFile file, string key)
    {
        const string rootPath = "PhotoStore";

        var wwwRootFolder = _env.WebRootPath;
        var userFolder = Path.Combine(wwwRootFolder, rootPath);
        var folder = Path.Combine(userFolder, key);

        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

        var fullFilePath = Path.Combine(folder, Path.GetFileName(file.FileName) ?? string.Empty);

        await using var targetStream = new FileStream(fullFilePath, FileMode.Create);
        await file.CopyToAsync(targetStream);
        targetStream.Close();
    }
}