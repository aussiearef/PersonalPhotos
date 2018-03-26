using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IFileStorage
    {
        Task StoreFile(IFormFile file, string key);
    }
}