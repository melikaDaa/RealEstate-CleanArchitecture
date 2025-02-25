using Microsoft.AspNetCore.Http;
using RealEstateApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Application.Interfaces
{
    public interface IEstateService
    {


        Task<IEnumerable<EstateDto>> GetAllEstatesAsync();
        Task<EstateDto> AddEstateAsync(EstateCreateDto estateDto);
        Task<EstateDto> UpdateEstateAsync(int id, EstateCreateDto estateDto);
         Task DeleteEstateAsync(int id);
         Task<IEnumerable<EstateDto>> SearchEstatesAsync(SearchDto searchDto);




    }
}
