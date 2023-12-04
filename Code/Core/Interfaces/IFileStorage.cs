using System.Threading.Tasks;
using Microsoft.AspNetCore.Http ;

namespace Core.Interfaces;

public interface IFileStorage
{
    Task StoreFile(Microsoft.AspNetCore.Http.FormFile file, string key);
}