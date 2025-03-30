using Core.Interfaces;

namespace Core.Services
{
    public class DefaultFileOperations : IFileOperations
    {
        public string Combine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public FileStream CreateFileStream(string filePath)
        {
            return new FileStream(filePath, FileMode.Create);
        }

        public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }
    }
}
