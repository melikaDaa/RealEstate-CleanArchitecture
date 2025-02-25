using AutoMapper;
using RealEstateApp.Application.DTOs;
using RealEstateApp.Infrastructure.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Infrastructure.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // تبدیل از ApplicationUser به UserDto و برعکس
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));  // Map کردن FullName
            CreateMap<UserDto, ApplicationUser>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));  // برعکس Map کردن FullName
            CreateMap<ApplicationUser, UserDetailsDto>()
    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

        }
    }
}
