using System.Threading.Tasks;
using Zwaj.api.Models;
namespace Zwaj.api.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user,string password);
         Task<User> Login(string username,string password);
         Task<bool> UserExits(string username);
    }
}