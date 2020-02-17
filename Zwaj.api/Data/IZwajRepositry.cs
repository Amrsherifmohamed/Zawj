using System.Collections.Generic;
using System.Threading.Tasks;
using Zwaj.api.Models;
using Zwaj.api.Dtos;

namespace Zwaj.api.Data
{
    public interface IZwajRepositry
    {
         void Add <T>(T entity) where T:class;
         //this defination can you make add any entity
         void Delete <T>(T entity) where T:class;
         Task<bool> SaveAll();
         Task<IEnumerable<User>> GetUsers();
         Task<User> GetUser(int id);   
         Task<Photo> GetPhoto(int id);  
         Task<Photo> GetmainPhotoForUser(int id);

    }
}