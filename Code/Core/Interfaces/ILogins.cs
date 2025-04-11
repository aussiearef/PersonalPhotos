using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces;

public interface ILogins
{
    Task CreateUser(string email, string password, CancellationToken token);
    Task<User?> GetUser(string email, CancellationToken token);
    Task<UserLoginResult> Login(string email, string password, CancellationToken token);
}