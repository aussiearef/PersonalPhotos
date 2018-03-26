using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IPhotoMetaData
    {
        Task SavePhotoMetaData(string userName, string desciption, string fileName);
        Task<List<PhotoModel>> GetUserPhotos(string userName);
    }
}