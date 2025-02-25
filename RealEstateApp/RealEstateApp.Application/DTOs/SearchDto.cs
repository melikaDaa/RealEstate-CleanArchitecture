using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Application.DTOs
{
    public class SearchDto
    {
        public string? Title { get; set; }
        public string? Address { get; set; }
        public int? CategoryId { get; set; }
        public double? MaxPrice { get; set; }
    }

}
