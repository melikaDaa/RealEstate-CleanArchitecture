using AutoMapper;
using RealEstateApp.Application.DTOs;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Application.Repositories;

namespace RealEstateApp.Infrastructure.Services
{
    /// <summary>
    /// Service for managing categories, handling business logic and interaction with the data layer.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork<Category> _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork<Category> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Retrieves all categories from the data store.
        /// </summary>
        /// <returns>A list of CategoryDto objects.</returns>
        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.GetAllAsync();
            return MapCategoriesToDto(categories);
        }

        /// <summary>
        /// Retrieves a single category by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the category.</param>
        /// <returns>A CategoryDto object.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the category is not found.</exception>
        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await FindCategoryByIdAsync(id);
            return MapCategoryToDto(category);
        }

        /// <summary>
        /// Adds a new category to the data store.
        /// </summary>
        /// <param name="categoryDto">The DTO representing the category to be added.</param>
        public async Task AddCategoryAsync(CategoryDto categoryDto)
        {
            var category = MapDtoToCategory(categoryDto);
            await _unitOfWork.AddAsync(category);
        }

        /// <summary>
        /// Updates an existing category by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the category to be updated.</param>
        /// <param name="categoryDto">The DTO representing the updated category data.</param>
        /// <exception cref="KeyNotFoundException">Thrown when the category is not found.</exception>
        public async Task UpdateCategoryAsync(int id, CategoryDto categoryDto)
        {
            var existingCategory = await FindCategoryByIdAsync(id);
            _mapper.Map(categoryDto, existingCategory);
            await _unitOfWork.UpdateAsync(existingCategory, id);
        }

        /// <summary>
        /// Deletes an existing category by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the category to be deleted.</param>
        /// <exception cref="KeyNotFoundException">Thrown when the category is not found.</exception>
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await FindCategoryByIdAsync(id);
            await _unitOfWork.DeleteAsync(category);
        }

        #region Private helper methods

        private async Task<Category> FindCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.GetByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }
            return category;
        }

        private CategoryDto MapCategoryToDto(Category category)
        {
            return _mapper.Map<CategoryDto>(category);
        }

        private List<CategoryDto> MapCategoriesToDto(List<Category> categories)
        {
            return _mapper.Map<List<CategoryDto>>(categories);
        }

        private Category MapDtoToCategory(CategoryDto categoryDto)
        {
            return _mapper.Map<Category>(categoryDto);
        }

        #endregion
    }
}
