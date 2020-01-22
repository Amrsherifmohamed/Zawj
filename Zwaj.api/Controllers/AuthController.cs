using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Zwaj.api.Data;
using Zwaj.api.Dtos;
using Zwaj.api.Models;

namespace Zwaj.api.Controllers
{
    [AllowAnonymous] // any one can inter in this controller
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost("register")]
        public async Task<IActionResult> register(UserForRegiterDto userForRegiterDto)
        {
            if (await _repo.UserExits(userForRegiterDto.Username))
                return BadRequest("His Username Is Exits");
            var usertocreated = new User
            {
                Username = userForRegiterDto.Username
            };
            var usercreated = await _repo.Register(usertocreated, userForRegiterDto.Password);
            return StatusCode(201);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
           
                // throw new Exception ("exaption");
                 var userFromrepo = await _repo.Login(userForLoginDto.username.ToLower(), userForLoginDto.password);
            if (userFromrepo == null) return Unauthorized();
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier,userFromrepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userFromrepo.Username)
        };
        var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Appsettings:Token").Value));
        var cards=new SigningCredentials(key,SecurityAlgorithms.HmacSha512);
        var tokenDescriptor=new SecurityTokenDescriptor{
            Subject=new ClaimsIdentity(claims),
            Expires=DateTime.Now.AddDays(1),
            SigningCredentials=cards
        };
        var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new{
                token = tokenHandler.WriteToken(token)
            });
            
    }

}
}