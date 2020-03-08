using System.Collections.Generic;
using System.Threading.Tasks;
using Zwaj.api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Zwaj.api.helper;
using System;

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
        // public async  Task<PagedList<User>> GetUsers(UserParems userParems){
        //     var user= _context.Users.Include(u=>u.Photos);
        //     return await PagedList<User>.
        //     CreateAsync(user,userParems.PageNumber,userParems.pageSize);
        // }
          public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
           var users =  _context.Users.Include(u=>u.Photos)
           .OrderByDescending(u=>u.lastActive).AsQueryable();
           users = users.Where(u=>u.Id!=userParams.UserId);
           users = users.Where(u=>u.Gender==userParams.Gender);
           if(userParams.Likers){
               var userLiker=await GetuserLikes(userParams.UserId,userParams.Likers);
               users=users.Where(u=>userLiker.Contains(u.Id));
           }
           if(userParams.Likees){
               var userLikee=await GetuserLikes(userParams.UserId,userParams.Likers);
               users=users.Where(u=>userLikee.Contains(u.Id));
           }
           if(userParams.MinAge!=18||userParams.MaxAge!=99){
               var minDob = DateTime.Today.AddYears(-userParams.MaxAge-1);
               var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
               users = users.Where(u=>u.DateofBirth>=minDob && u.DateofBirth<=maxDob);
           }
           if(!string.IsNullOrEmpty(userParams.OrderBy)){
               switch (userParams.OrderBy)
               {
                   case "created":
                   users=users.OrderByDescending(u=>u.created);
                   break;
                   default:
                   users= users.OrderByDescending(u=>u.lastActive);
                   break;
               }
           }
           return await PagedList<User>.CreateAsync(users,userParams.PageNumber,userParams.PageSize);
        }
        public async Task<User> GetUser(int id,bool isCurrentuser)
        {
            var query=_context.Users.Include(u=>u.Photos).AsQueryable();
            if(isCurrentuser)
                    query=query.IgnoreQueryFilters();
              var user= await query.FirstOrDefaultAsync(u=>u.Id==id);
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

        public async Task<Like> GetLike(int userId, int recipientId)
        {
            return await _context.Likes
            .FirstOrDefaultAsync(l=>l.LikerId==userId&&l.LikeeId==recipientId);
        }
        private async Task<IEnumerable<int>> GetuserLikes(int id ,bool likers){
            var user=await _context.Users.Include(l=>l.Likers).Include(l=>l.Likees)
            .FirstOrDefaultAsync(i=>i.Id==id);
            if(likers){
                return user.Likers.Where(u=>u.LikeeId==id).Select(l=>l.LikerId);
            }
            else{
                return user.Likees.Where(u=>u.LikerId==id).Select(l=>l.LikeeId);
            }
        }

        public async Task<Message> GetMessage(int id)
        {
            var messagereturn=await _context.Messages.FirstOrDefaultAsync(m=>m.Id==id);
            return messagereturn;
        }

        public async Task<PagedList<Message>> GetMessageForUser(MessageParams messageParams)
        {
            var messages= _context.Messages.Include(m=>m.Sender).ThenInclude(u=>u.Photos)
            .Include(m=>m.Recipient).ThenInclude(u=>u.Photos).AsQueryable();
            switch (messageParams.MessageType)
            {
                case "Inbox":
                messages=messages.Where(m=>m.RecipientId==messageParams.UserId&&
                m.RecipientDeleted==false);
                break;
                case "Outbox":
                messages=messages.Where(m=>m.SenderId==messageParams.UserId&&
                m.SenderDelated==false);
                break;
                default:
                messages=messages.Where(m=>m.RecipientId==messageParams.UserId&&m.IsRead==false
                &&m.RecipientDeleted==false);
                break;
            }
            messages=messages.OrderByDescending(m=>m.MessageSent);
            return await PagedList<Message>.CreateAsync(messages,messageParams.PageNumber,messageParams.PageSize);
                
        }
        public async Task<IEnumerable<Message>> GetConversition(int userId, int recipientId)
        {
            var messages = await _context.Messages.Include(m=>m.Sender).
            ThenInclude(u=>u.Photos)
            .Include(m=>m.Recipient).ThenInclude(u=>u.Photos)
            .Where(m=>m.RecipientId==userId &&m.RecipientDeleted==false&&
             m.SenderId==recipientId || m.RecipientId==recipientId &&m.SenderDelated==false&& m.SenderId==userId)
             .OrderByDescending(m=>m.MessageSent).ToListAsync();
            return messages;
           // && m.RecipientDeleted == false
           // m.SenderDeleted == false &&
        }
        public async Task<int> GetUnreadMessagesForUser(int userId)
        {
            var messages = await _context.Messages.Where(m => m.IsRead == false && m.RecipientId == userId).ToListAsync();
            var count = messages.Count();
            return count;

        }

        public async Task<Payment> GetPaymentForuser(int userId)
        {
           return await _context.Payments.FirstOrDefaultAsync(p=>p.UserId==userId);
        }
    }
}