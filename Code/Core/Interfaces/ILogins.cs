using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface ILogins
    {
        Task CreateUser(string email, string password);
        Task<User> GetUser(string email);
    }
}