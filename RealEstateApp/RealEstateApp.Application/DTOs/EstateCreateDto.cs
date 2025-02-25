using Microsoft.AspNetCore.Http;

namespace RealEstateApp.Application.DTOs
{
    public class EstateCreateDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Metrage { get; set; }
        public IFormFile Image { get; set; } // دریافت تصویر به عنوان IFormFile
        public double Price { get; set; }
        public string Address { get; set; }
        public int? CategoryId { get; set; } // دسته‌بندی

    }
}
