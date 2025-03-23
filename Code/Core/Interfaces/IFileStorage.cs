using System.Threading.Tasks;
using Microsoft.AspNetCore.Http ;

namespace Core.Interfaces;

public interface IFileStorage
{
    Task StoreFile(Microsoft.AspNetCore.Http.IFormFile file, string key);
}