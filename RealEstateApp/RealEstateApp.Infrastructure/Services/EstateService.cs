using AutoMapper;
using RealEstateApp.Application.DTOs;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using RealEstateApp.Application.Repositories;

namespace RealEstateApp.Infrastructure.Services
{
    public class EstateService : IEstateService
    {
        private readonly IUnitOfWork<Estates> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public EstateService(IMapper mapper, IUnitOfWork<Estates> unitOfWork, ApplicationDbContext context)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<EstateDto>> GetAllEstatesAsync()
        {
            var estates = await _context.Estate
                .Include(e => e.Category) // بارگذاری اطلاعات دسته‌بندی
                .ToListAsync();

            return _mapper.Map<IEnumerable<EstateDto>>(estates);
        }

        // متد برای اضافه کردن ملک جدید
        public async Task<EstateDto> AddEstateAsync(EstateCreateDto estateDto)
        {
            var estate = _mapper.Map<Estates>(estateDto);

            if (estateDto.Image != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    // تصویر را از IFormFile به یک آرایه بایت تبدیل می‌کنیم
                    await estateDto.Image.CopyToAsync(memoryStream);
                    estate.Image = memoryStream.ToArray(); // ذخیره به صورت byte[]
                }
            }

            await _unitOfWork.AddAsync(estate);
            await _unitOfWork.SaveChangesAsync();  // Ensure the changes are saved to the DB

            return _mapper.Map<EstateDto>(estate);  // Map the estate entity back to EstateDto
        }

        // متد برای ویرایش (آپدیت) ملک
        public async Task<EstateDto> UpdateEstateAsync(int id, EstateCreateDto estateDto)
        {
            var estate = await _context.Estate
                .Include(e => e.Category) // Ensure the category is loaded for updating
                .FirstOrDefaultAsync(e => e.Id == id);

            if (estate == null)
                throw new KeyNotFoundException("Estate not found.");

            // Use AutoMapper to map the update DTO to the estate entity
            _mapper.Map(estateDto, estate);

            if (estateDto.Image != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await estateDto.Image.CopyToAsync(memoryStream);
                    estate.Image = memoryStream.ToArray(); // Update the image
                }
            }

            // Save changes to the database
            _context.Estate.Update(estate);
            await _context.SaveChangesAsync();

            return _mapper.Map<EstateDto>(estate);  // Return the updated EstateDto
        }
        public async Task<IEnumerable<EstateDto>> SearchEstatesAsync(SearchDto searchDto)
        {
            var query = _context.Estate.AsQueryable();

            if (!string.IsNullOrEmpty(searchDto.Title))
                query = query.Where(p => p.Title.Contains(searchDto.Title));

            if (!string.IsNullOrEmpty(searchDto.Address))
                query = query.Where(p => p.Address.Contains(searchDto.Address));

            if (searchDto.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == searchDto.CategoryId);

            if (searchDto.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= searchDto.MaxPrice);

            var results = await query.ToListAsync();

            return _mapper.Map<IEnumerable<EstateDto>>(results); // Assuming you are using AutoMapper to map the entities to DTOs
        }

        // متد برای حذف ملک
        public async Task DeleteEstateAsync(int id)
        {
            var estate = await _context.Estate
                .FirstOrDefaultAsync(e => e.Id == id);

            if (estate == null)
                throw new KeyNotFoundException("Estate not found.");

            _context.Estate.Remove(estate);
            await _context.SaveChangesAsync();
        }
    }
}
