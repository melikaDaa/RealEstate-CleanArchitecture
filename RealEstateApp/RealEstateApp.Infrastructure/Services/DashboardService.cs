using RealEstateApp.Application.DTOs;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Infrastructure.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.DTOs;

namespace RealEstateApp.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StatsDto> GetStatsAsync()
        {
            var totalEstates = await _context.Estate.CountAsync();
            var totalCategories = await _context.Category.CountAsync();
            var totalUsers = await _context.Users.CountAsync(); // اگر جدول Users همین اسم رو داره

            return new StatsDto
            {
                TotalEstates = totalEstates,
                TotalCategories = totalCategories,
                TotalUsers = totalUsers
            };
        }
    }
}
