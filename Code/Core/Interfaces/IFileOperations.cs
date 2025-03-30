namespace Core.Interfaces
{
    public interface IFileOperations
    {
        string Combine(string path1, string path2);
        bool DirectoryExists(string path);
        void CreateDirectory(string path);
        FileStream CreateFileStream(string filePath);
        string GetFileName(string path);
    }
}
