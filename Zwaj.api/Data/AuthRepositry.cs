using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZwajApp.API.Data;
using ZwajApp.API.Models;

namespace Zwaj.api.Data
{
    public class AuthRepositry : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepositry(DataContext context)
        {
            _context = context;
            
        }
       public async Task<User> Register(User user,string password){
            byte[] passwordhash,passwordsolt;
            createpasswordhash(password,out passwordhash,out passwordsolt);  
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user; 
        }
        private void createpasswordhash(string password ,out byte[] passwordhash,out byte[] passwordsolt)
        {
            using(var hmac=new System.Security.Cryptography.HMACSHA512()){
                passwordsolt=hmac.Key;
                passwordhash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public async Task<User> Login(string username,string password){
            var user=await _context.Users.Include(p=>p.Photos).FirstOrDefaultAsync(x=>x.UserName==username);
            if(user==null) return null;
            return user;
        }
        private bool VerifyPasswordHash(string password,byte[] passwordsolt,byte[] passwordhash){
             using(var hmac=new System.Security.Cryptography.HMACSHA512(passwordsolt)){              
                var computehash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i=0;i<passwordhash.Length;i++){
                    if(computehash[i]!=passwordhash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public async Task<bool> UserExits(string username){
            if(await _context.Users.AnyAsync(x=>x.UserName==username)) return true;
            return false;  
        }

        public Task<bool> UserExists(string username)
        {
            throw new System.NotImplementedException();
        }
    }
}