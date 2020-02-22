using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using Zwaj.api.Data;
using Zwaj.api.Dtos;
using Zwaj.api.Models;
using AutoMapper;
using Zwaj.api.helper;

namespace Zwaj.api.Controllers
{
   // [AllowAnonymous] dont need this becouse its have information about client 
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]//route for this controller api/Users
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IZwajRepositry _repo;
        private readonly IMapper _mapper;
        public UsersController(IZwajRepositry repo,IMapper mapper)
        {
            _repo=repo;  
            _mapper=mapper; 
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser(currentUserId);
            userParams.UserId=currentUserId;
            if(string.IsNullOrEmpty(userParams.Gender)){
                userParams.Gender=userFromRepo.Gender=="رجل"?"إمرأة":"رجل";
            }
            var users = await _repo.GetUsers(userParams);
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            Response.AddPagination(users.CurrentPage,users.PageSize,users.TotalCount,users.TotalPages);
            return Ok(usersToReturn);
        }

        [HttpGet("{id}",Name="GetUser")]
        public async Task<IActionResult> GetUser(int id){
            var userfromsorce =await _repo.GetUser(id);
            var usertodistnation= _mapper.Map<UserForDetailsDto>(userfromsorce);
            return Ok(usertodistnation);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id,UserForUpdateDto userForUpdateDto){
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();
            var userFromRepo=await _repo.GetUser(id);
            _mapper.Map(userForUpdateDto,userFromRepo);
            if(await _repo.SaveAll()){
                return NoContent();
            }
            throw new Exception($"حدثت مشكله فى تعديل بيانات المشترك رقم {id}");
        }
        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id ,int recipientId){
            if(id!= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();
            var like=await _repo.GetLike(id,recipientId);
            if(like!=null){
                return BadRequest("هذه المسخدم موجود فى قائمة الاعجاب من قبل ");
            }
            if(await _repo.GetUser(recipientId)==null)
            return NotFound();
            like=new Like{
                LikerId=id,
                LikeeId=recipientId
            };
            _repo.Add<Like>(like);
           if(await _repo.SaveAll()){
           return Ok();}else{
               return BadRequest("حدث خطا ما اثناء الاعجاب");
           }
        }

    }
}