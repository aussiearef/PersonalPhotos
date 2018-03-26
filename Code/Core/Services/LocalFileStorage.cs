using System.IO;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Core.Services
{
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
            var folder = $"{_env.WebRootPath}\\{rootPath}\\{key}";
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            var fullFilePath = $"{folder}\\{Path.GetFileName(file.FileName)}";

            using (var targetStream = new FileStream(fullFilePath, FileMode.Create))
            {
                await file.CopyToAsync(targetStream);
                targetStream.Close();
            }
        }
    }
}