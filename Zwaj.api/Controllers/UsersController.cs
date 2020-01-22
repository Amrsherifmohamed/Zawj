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

namespace Zwaj.api.Controllers
{
   // [AllowAnonymous] dont need this becouse its have information about client 
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
        public async Task<IActionResult> GetUsers(){
            var userfromsorce=await _repo.GetUsers();
            var usertodistnation = _mapper.Map<IEnumerable<UserForListDto>>(userfromsorce);
            return Ok(usertodistnation);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id){
            var userfromsorce =await _repo.GetUser(id);
            var usertodistnation= _mapper.Map<UserForDetailsDto>(userfromsorce);
            return Ok(usertodistnation);
        }

    }
}