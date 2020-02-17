using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Zwaj.api.Data;
using Zwaj.api.Dtos;
using Zwaj.api.Models;
using Zwaj.api.helper;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet.Core;
using System.Linq;

namespace Zwaj.api.Controllers
{
  [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IZwajRepositry _repo;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private readonly IMapper _mapper;
        private Cloudinary _cloudinary;

        public PhotosController(IZwajRepositry repo, IOptions<CloudinarySettings> cloudinaryConfig,
        IMapper mapper)
        {
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;
            _repo = repo;
            Account account  = new Account(
               _cloudinaryConfig.Value.Cloudname,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );
              
               _cloudinary = new Cloudinary(account);

        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepository = await _repo.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepository);
            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId,[FromForm]PhotoForCreateDto photoForCreateDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var userFromRepo = await _repo.GetUser(userId);
            var file = photoForCreateDto.File;
            var uploadResult = new ImageUploadResult();
            if (file != null && file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                       .Width(500).Height(500).Crop("fill").Gravity("face")
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }

            }
            photoForCreateDto.Url = uploadResult.Uri.ToString();
            photoForCreateDto.publicId = uploadResult.PublicId;
            var photo = _mapper.Map<Photo>(photoForCreateDto);
            if (!userFromRepo.Photos.Any(p => p.Ismain))
                photo.Ismain = true;
            userFromRepo.Photos.Add(photo);
            if (await _repo.SaveAll())
            {
                var PhotoToReturn =_mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.Id },PhotoToReturn);
            }

            return BadRequest("خطأ في إضافة الصورة");

        }
        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId,int id){
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var userFromRepo=await _repo.GetUser(userId);
            if(!userFromRepo.Photos.Any(p=>p.Id==id))
                return Unauthorized();
            var DesiredMainPhoto=await _repo.GetPhoto(id);
            if(DesiredMainPhoto.Ismain)
                return BadRequest("هذه الصوره الاساسيه بالفعل");
            var CurrentMainPhoto =await _repo.GetmainPhotoForUser(userId);
            CurrentMainPhoto.Ismain=false;
            DesiredMainPhoto.Ismain=true;
            if(await _repo.SaveAll())
            return NoContent();
            return BadRequest("لا يمكن تعديل الصوره");

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletephoto(int userId,int id){
             if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var userFromRepo=await _repo.GetUser(userId);
            if(!userFromRepo.Photos.Any(p=>p.Id==id))
                return Unauthorized();
            var Photo=await _repo.GetPhoto(id);
            if(Photo.Ismain)
                return BadRequest("هذه الصوره الاساسيه بالفعل");
            if(Photo.PublicId!=null){
                var deleteparems=new DeletionParams(Photo.PublicId);    
                var result=this._cloudinary.Destroy(deleteparems);
                if(result.Result=="OK"){
                    _repo.Delete(Photo);
                }
            }
            if(Photo.PublicId==null){
                this._repo.Delete(Photo);
            }
            if(await _repo.SaveAll())
                return Ok();
                return BadRequest("لم يتم مسح الصوره");
            
        }

    }   
}
