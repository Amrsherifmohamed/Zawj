using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zwaj.api.Data;
using Zwaj.api.Dtos;
using Zwaj.api.helper;
using Zwaj.api.Models;

namespace Zwaj.api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]//route for this controller api/Users
    [ApiController]
    public class MessagesController :ControllerBase
    {
         private readonly IZwajRepositry _repo;
        private readonly IMapper _mapper;
        public MessagesController(IZwajRepositry repo,IMapper mapper)
        {
            _repo=repo;  
            _mapper=mapper; 
        }
        [HttpGet("{id}",Name="GetMessage")]
        public async Task<IActionResult> GetMeassage(int userId,int id){
            if(userId!= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();
            var messageFromrepo=await _repo.GetMessage(id);
            if(messageFromrepo==null){
                return NotFound();
            }
            return Ok(messageFromrepo);
        }
        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId,[FromQuery]MessageParams messageParams )
        {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
             return Unauthorized();
             messageParams.UserId=userId;
             var MessagesFromRepo = await _repo.GetMessageForUser(messageParams);
             var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(MessagesFromRepo);
             Response.AddPagination(MessagesFromRepo.CurrentPage,MessagesFromRepo.PageSize,MessagesFromRepo.TotalCount,MessagesFromRepo.TotalPages);
             return Ok(messages);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId,MessageForCreateDto 
        messageForCreateDto)
        {
            var sender= _repo.GetUser(userId);
            if(sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();
            messageForCreateDto.SenderId=userId;
            var RecipentFromrepo= await _repo.GetUser(messageForCreateDto.RecipientId);
            if(RecipentFromrepo==null)
            return BadRequest("لم يتم الوصول للمرسال اليه");
            var message=_mapper.Map<Message>(messageForCreateDto);
            _repo.Add(message);
            if(await _repo.SaveAll()){
            var messagetoreturn=_mapper.Map<MessageToReturnDto>(message);
            return CreatedAtRoute("GetMessage",new {Id=message.Id},messagetoreturn);
            //id= id for message in new route from database
            }
            throw new Exception("حدثت مشكله فى حفظ الرساله ");
        }
        [HttpGet("chat/{recipientId}")]
        public async Task<IActionResult> GetConversation(int userId , int recipientId)
        {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
             return Unauthorized();
             var messagesFromRepo = await _repo.GetConversition(userId,recipientId);
             var messageToReturn = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);
             return Ok(messageToReturn);
        }
        [HttpGet("count")]
        public async Task<IActionResult> GetUnreadMessagesForUser(int userId){
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();
            var count = await _repo.GetUnreadMessagesForUser( int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            return Ok(count);
        }
        [HttpPost("read/{id}")]
        public async Task<IActionResult> MarkMessageAsRead(int userId,int id){
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
             return Unauthorized();
             var message = await _repo.GetMessage(id);
             if(message.RecipientId != userId)
                 return Unauthorized();
            message.IsRead = true;
            message.DataRead=DateTime.Now;
            await _repo.SaveAll();
            return NoContent();
       }	
       [HttpPost("{id}")]
       public async Task<IActionResult> DeleteMessage(int id,int userId){
         if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
             return Unauthorized();
         var message = await _repo.GetMessage(id);
         if(message.SenderId==userId)
        message.SenderDelated=true;
        if(message.RecipientId==userId)
        message.RecipientDeleted=true;
        if(message.SenderDelated&&message.RecipientDeleted)
        _repo.Delete(message);
        if(await _repo.SaveAll())
        return NoContent();
        throw new Exception ("حدث خطا اثناؤ المسح");
       }
        
    }
}