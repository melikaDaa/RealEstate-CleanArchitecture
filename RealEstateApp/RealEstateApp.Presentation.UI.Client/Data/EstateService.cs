using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using RealEstateApp.Application.DTOs;

namespace RealEstateApp.Presentation.UI.Client.Data
{
    public class EstateService
    {
        private readonly HttpClient _httpClient;

        public EstateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Get all estates
        public async Task<IEnumerable<EstateDto>> GetAllEstatesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<EstateDto>>("https://localhost:7044/api/estate");
        }

        // Add a new estate
        public async Task AddEstateAsync(EstateCreateDto1 estate)
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(estate.Title), "Title" },
                { new StringContent(estate.Description ?? ""), "Description" },
                { new StringContent(estate.Metrage.ToString()), "Metrage" },
                { new StringContent(estate.Price.ToString()), "Price" },
                { new StringContent(estate.Address), "Address" },
                { new StringContent(estate.CategoryId.ToString()), "CategoryId" }
            };

            if (estate.Image != null)
            {
                var imageContent = new ByteArrayContent(estate.Image); // Use byte array content
                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg"); // Adjust MIME type as needed
                content.Add(imageContent, "Image", "uploadedImage.jpg"); // "uploadedImage.jpg" is a placeholder file name
            }

            await _httpClient.PostAsync("https://localhost:7044/api/estate", content);
        }

        // Update an existing estate
        public async Task UpdateEstateAsync(int id, EstateCreateDto1 estateUpdateDto)
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(estateUpdateDto.Title), "Title" },
                { new StringContent(estateUpdateDto.Description ?? ""), "Description" },
                { new StringContent(estateUpdateDto.Metrage.ToString()), "Metrage" },
                { new StringContent(estateUpdateDto.Price.ToString()), "Price" },
                { new StringContent(estateUpdateDto.Address), "Address" },
                { new StringContent(estateUpdateDto.CategoryId.ToString()), "CategoryId" }
            };

            if (estateUpdateDto.Image != null)
            {
                var imageContent = new ByteArrayContent(estateUpdateDto.Image); // Use byte array content
                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg"); // Adjust MIME type as needed
                content.Add(imageContent, "Image", "updatedImage.jpg"); // "updatedImage.jpg" is a placeholder file name
            }

            // PUT request for updating an estate
            await _httpClient.PutAsync($"https://localhost:7044/api/estate/{id}", content);
        }
        public async Task<IEnumerable<EstateDto>> SearchEstatesAsync(SearchDto searchDto)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7044/api/estate/search", searchDto);
            return await response.Content.ReadFromJsonAsync<IEnumerable<EstateDto>>();
        }

        // Delete an estate
        public async Task DeleteEstateAsync(int id)
        {
            // DELETE request for deleting an estate
            await _httpClient.DeleteAsync($"https://localhost:7044/api/estate/{id}");
        }
        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CategoryDto>>("https://localhost:7044/api/categories");
            return response ?? new List<CategoryDto>();
        }

    }

    public class EstateCreateDto1
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Metrage { get; set; }
        public byte[] Image { get; set; } // For selecting an image file
        public double Price { get; set; }
        public string Address { get; set; }
        public int? CategoryId { get; set; }
    }

    public class EstateUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Metrage { get; set; }
        public byte[] Image { get; set; } // For selecting an image file
        public double Price { get; set; }
        public string Address { get; set; }
        public int? CategoryId { get; set; }
    }
}
