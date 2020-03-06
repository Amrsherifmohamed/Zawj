using System.Collections.Generic;
using System.Threading.Tasks;
using Zwaj.api.Models;
using Zwaj.api.Dtos;
using Zwaj.api.helper;

namespace Zwaj.api.Data
{
    public interface IZwajRepositry
    {
         void Add <T>(T entity) where T:class;
         //this defination can you make add any entity
         void Delete <T>(T entity) where T:class;
         Task<bool> SaveAll();
         Task<PagedList<User>> GetUsers(UserParams userParams);
         Task<User> GetUser(int id);   
         Task<Photo> GetPhoto(int id);  
         Task<Photo> GetmainPhotoForUser(int id);
         Task<Like> GetLike(int userId,int recipientId);
         Task<Message> GetMessage(int id);
         Task<PagedList<Message>> GetMessageForUser(MessageParams messageParams);
         Task<IEnumerable<Message>> GetConversition(int UserId,int recipienId); 
         Task<int> GetUnreadMessagesForUser(int userId);
         Task<Payment>  GetPaymentForuser(int userId);   

    }
}