using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Application.DTOs
{
    public class EstateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Metrage { get; set; }
        public string Image { get; set; } // تصویر به صورت Base64
        public double Price { get; set; }
        public string Address { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }  // اضافه کردن فیلد CategoryName
    }

}
