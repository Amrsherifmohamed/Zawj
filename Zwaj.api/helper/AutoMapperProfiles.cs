using AutoMapper;
using Zwaj.api.Dtos;
using Zwaj.api.Models;
using System.Linq;
namespace Zwaj.api.helper
{
    public  class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User,UserForDetailsDto>()
            .ForMember(dest=>dest.photourl,opt=>{opt.MapFrom(src=>src.Photos.FirstOrDefault(p=>p.Ismain).Url);})
            .ForMember(dest=>dest.age,opt=>{opt.ResolveUsing(src=>src.DateofBirth.calculateage());});
            CreateMap<User,UserForListDto>()
            .ForMember(dest=>dest.photourl,opt=>{opt.MapFrom(src=>src.Photos.FirstOrDefault(p=>p.Ismain).Url);})
            .ForMember(dest=>dest.age,opt=>{opt.ResolveUsing(src=>src.DateofBirth.calculateage());});
            CreateMap<Photo,PhotoForDetailsDto>();
        }
    }
}  