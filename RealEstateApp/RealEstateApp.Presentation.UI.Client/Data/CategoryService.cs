using RealEstateApp.Application.DTOs;

namespace RealEstateApp.Presentation.UI.Client.Data
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CategoryDto>>("https://localhost:7044/api/categories");
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<CategoryDto>($"https://localhost:7044/api/categories/{id}");
        }

        public async Task AddCategoryAsync(CategoryDto categoryDto)
        {
            await _httpClient.PostAsJsonAsync("https://localhost:7044/api/categories", categoryDto);
        }

        public async Task UpdateCategoryAsync(int id, CategoryDto categoryDto)
        {
            await _httpClient.PutAsJsonAsync($"https://localhost:7044/api/categories/{id}", categoryDto);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _httpClient.DeleteAsync($"https://localhost:7044/api/categories/{id}");
        }
    }
    }
