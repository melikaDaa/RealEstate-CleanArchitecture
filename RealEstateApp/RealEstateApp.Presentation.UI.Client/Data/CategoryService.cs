using RealEstateApp.Application.DTOs;
using RealEstateApp.Presentation.UI.Client.Errors;

namespace RealEstateApp.Presentation.UI.Client.Data
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly GlobalErrorHandler _globalErrorHandler;

        public CategoryService(HttpClient httpClient, GlobalErrorHandler globalErrorHandler)
        {
            _httpClient = httpClient;
            _globalErrorHandler = globalErrorHandler;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CategoryDto>>("https://localhost:7044/api/categories");
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            return await _globalErrorHandler.HandleRequestAsync<CategoryDto>(() =>
                _httpClient.GetAsync("https://localhost:7044/api/categories/{id}")
            );
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
