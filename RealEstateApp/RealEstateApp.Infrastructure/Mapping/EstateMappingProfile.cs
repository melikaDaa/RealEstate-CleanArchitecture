using AutoMapper;
using RealEstateApp.Application.DTOs;
using RealEstateApp.Domain.Entities;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace RealEstateApp.Infrastructure.Mapping
{
    public class EstateMappingProfile : Profile
    {
        public EstateMappingProfile()
        {
            CreateMap<EstateCreateDto, Estates>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => ConvertFormFileToByteArray(src.Image))); // تبدیل دستی از FormFile به byte[]

            CreateMap<Estates, EstateDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Title))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image)));
        }

        // متد تبدیل FormFile به byte[]
        private static byte[] ConvertFormFileToByteArray(IFormFile formFile)
        {
            if (formFile == null) return null;

            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
