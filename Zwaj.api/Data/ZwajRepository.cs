using System.Collections.Generic;
using System.Threading.Tasks;
using Zwaj.api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Zwaj.api.Data
{
    public class ZwajRepository : IZwajRepositry
    {
        private readonly Datacontext _context;
        public ZwajRepository(Datacontext context)
        {
            _context=context;
        }
       public void Add <T>(T entity) where T:class
         {
             _context.Add(entity);
         }
       public void Delete <T>(T entity) where T:class
         {
             _context.Remove(entity);
         }
       public async Task<bool> SaveAll()
       {
           return await _context.SaveChangesAsync()>0;
       }
        public async  Task<IEnumerable<User>> GetUsers(){
            var user=await _context.Users.Include(u=>u.Photos).ToListAsync();
            return user;
        }
        public async Task<User> GetUser(int id)
        {
              var user= await _context.Users.Include(u=>u.Photos).FirstOrDefaultAsync(u=>u.Id==id);
                return user;
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo=await _context.Photos.FirstOrDefaultAsync(p=>p.Id==id);
            return photo;

        }
        public async Task<Photo> GetmainPhotoForUser(int id){
            return await _context.Photos.Where(u=>u.UserId==id).FirstOrDefaultAsync(p=>p.Ismain);
        }   
    }
}