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
using Microsoft.Extensions.Options;
using Stripe;
// using Stripe;

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
        private readonly IOptions<StripeSettings> _stripSetteings;
        public UsersController(IZwajRepositry repo,IMapper mapper,IOptions<StripeSettings> stripSetteings)
        {
            _repo=repo;  
            _mapper=mapper; 
            _stripSetteings=stripSetteings;
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
        [HttpPost("{userId}/charge/{stripeToken}")]
        public async Task<IActionResult> Charge(int userId, string stripeToken)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var customers = new CustomerService();
            var charges = new ChargeService();
            var customer = customers.Create(new CustomerCreateOptions {
            Source=stripeToken
            });
            var charge = charges.Create(new ChargeCreateOptions {
            Amount = 5000,
            Description = "إشتراك مدى الحياة",
            Currency = "usd",
            Customer = customer.Id
            });
            var payment = new Payment{
                PaymentDate = DateTime.Now,
                Amount = charge.Amount/100,
                UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                ReceiptUrl = charge.ReceiptUrl,
                Description = charge.Description,
                Currency = charge.Currency,
                IsPaid = charge.Paid
            };
            _repo.Add<Payment>(payment);
            if(await _repo.SaveAll()){
           return Ok(new {IsPaid = charge.Paid } );
            }
            return BadRequest("فشل في السداد");
        }
        [HttpGet("{userId}/payment")]
        public async Task<IActionResult> getpayment(int userId){
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var paymentId=await _repo.GetPaymentForuser(userId);
            if(paymentId!=null)
            return NoContent();
            return Ok(paymentId);
        }
    }
}