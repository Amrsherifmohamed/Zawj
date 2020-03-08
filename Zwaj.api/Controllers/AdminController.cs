using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zwaj.api.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Zwaj.api.Models;
using Zwaj.api.Dtos;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Zwaj.api.helper;
using CloudinaryDotNet;

namespace Zwaj.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly Datacontext _context;
        private readonly UserManager<User> _userManger;
                private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public AdminController(Datacontext context, UserManager<User> userManger
        , IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _userManger = userManger;
            _context = context;
             Account account  = new Account(
               _cloudinaryConfig.Value.Cloudname,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );
              
               _cloudinary = new Cloudinary(account);
        }
    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("userWithRoles")]
    public async Task<IActionResult> GetUsersWithRoles()
    {
        var userList = await
         (from user in _context.Users
          orderby user.UserName
          select new
          {
              Id = user.Id,
              username = user.UserName,
              Roles = (from userRole in user.UserRoles
                       join role in _context.Roles
                           on userRole.RoleId equals role.Id
                       select role.Name)
                      .ToList()
          }).ToListAsync();
        return Ok(userList);
    }
    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("editroles/{username}")]
    public async Task<IActionResult> EditRoles(string username, RoleEditDto roleEditDto)
    {
        var user = await _userManger.FindByNameAsync(username);
        var userRoles = await _userManger.GetRolesAsync(user);
        var selectedrole = roleEditDto.RoleNames;
        selectedrole = selectedrole ?? new string[] { };
        var result = await _userManger.AddToRolesAsync(user, selectedrole.Except(userRoles));
        if (!result.Succeeded)
            return BadRequest("فشل فى اضافه القواعد الجديده");
        result = await _userManger.RemoveFromRolesAsync(user, userRoles.Except(selectedrole));
        if (!result.Succeeded)
            return BadRequest("فشل فى ازالة تلك القواعد ");
        return Ok(await _userManger.GetRolesAsync(user));
    }
     [Authorize(Policy = "ModeratorPhotoRole")]
    [HttpGet("photosForModeration")]
   
    public async Task<IActionResult> GetPhotosForModeration()
    {
        var photos = await _context.Photos
            .Include(u => u.User)
            .IgnoreQueryFilters()
            .Where(p => p.IsApproved == false)
            .Select(u => new
            {
                Id = u.Id,
                UserName = u.User.UserName,
                KnownAs = u.User.KnownAs,
                Url = u.Url,
                IsApproved = u.IsApproved
            }).ToListAsync();

        return Ok(photos);
    }
    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpPost("approvePhoto/{photoId}")]
    public async Task<IActionResult> ApprovePhoto(int photoId)
    {
        var photo = await _context.Photos
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == photoId);

        photo.IsApproved = true;

        await _context.SaveChangesAsync();

        return Ok();
    }
    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpPost("rejectPhoto/{photoId}")]
    public async Task<IActionResult> RejectPhoto(int photoId)
    {
        var photo = await _context.Photos
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == photoId);

        if (photo.Ismain)
            return BadRequest("لا يمكنك رفض الصورة الأساسية");

        if (photo.PublicId != null)
        {
            var deleteParams = new DeletionParams(photo.PublicId);

           var result = _cloudinary.Destroy(deleteParams);

            if (result.Result == "ok")
            {
                _context.Photos.Remove(photo);
            }
        }

        if (photo.PublicId == null)
        {
            _context.Photos.Remove(photo);
        }

        await _context.SaveChangesAsync();

        return Ok();
    }


}
}