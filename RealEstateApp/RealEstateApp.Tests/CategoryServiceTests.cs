using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using RealEstateApp.Application.DTOs;
using RealEstateApp.Application.Repositories;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Services;
using Xunit;

namespace RealEstateApp.Tests
{
    public class CategoryServiceTests
    {
        private readonly Mock<IUnitOfWork<Category>> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork<Category>>();
            _mapperMock = new Mock<IMapper>();
            _service = new CategoryService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetCategoryByIdAsync_CategoryExists_ReturnsCategoryDto()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category { Id = categoryId, Title = "Test", Description = "Desc" };
            var categoryDto = new CategoryDto { Id = categoryId, Title = "Test", Description = "Desc" };

            _unitOfWorkMock.Setup(u => u.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mapperMock.Setup(m => m.Map<CategoryDto>(category)).Returns(categoryDto);

            // Act
            var result = await _service.GetCategoryByIdAsync(categoryId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(categoryDto);
        }

        [Fact]
        public async Task GetCategoryByIdAsync_CategoryDoesNotExist_ThrowsKeyNotFoundException()
        {
            // Arrange
            var categoryId = 99;
            _unitOfWorkMock.Setup(u => u.GetByIdAsync(categoryId)).ReturnsAsync((Category)null);

            // Act
            Func<Task> act = async () => await _service.GetCategoryByIdAsync(categoryId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Category with ID {categoryId} not found.");
        }

        [Fact]
        public async Task AddCategoryAsync_ValidCategoryDto_CallsAddAsync()
        {
            // Arrange
            var categoryDto = new CategoryDto { Id = 2, Title = "New", Description = "New Desc" };
            var category = new Category { Id = 2, Title = "New", Description = "New Desc" };

            _mapperMock.Setup(m => m.Map<Category>(categoryDto)).Returns(category);
            _unitOfWorkMock.Setup(u => u.AddAsync(category)).ReturnsAsync(category);

            // Act
            await _service.AddCategoryAsync(categoryDto);

            // Assert
            _unitOfWorkMock.Verify(u => u.AddAsync(category), Times.Once);
        }

        [Fact]
        public async Task UpdateCategoryAsync_CategoryExists_UpdatesCategory()
        {
            // Arrange
            var categoryId = 3;
            var categoryDto = new CategoryDto { Id = categoryId, Title = "Updated", Description = "Updated Desc" };
            var existingCategory = new Category { Id = categoryId, Title = "Old", Description = "Old Desc" };

            _unitOfWorkMock.Setup(u => u.GetByIdAsync(categoryId)).ReturnsAsync(existingCategory);
            _mapperMock.Setup(m => m.Map(categoryDto, existingCategory)).Verifiable();
            _unitOfWorkMock.Setup(u => u.UpdateAsync(existingCategory, categoryId)).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateCategoryAsync(categoryId, categoryDto);

            // Assert
            _mapperMock.Verify(m => m.Map(categoryDto, existingCategory), Times.Once);
            _unitOfWorkMock.Verify(u => u.UpdateAsync(existingCategory, categoryId), Times.Once);
        }

        [Fact]
        public async Task DeleteCategoryAsync_CategoryExists_DeletesCategory()
        {
            // Arrange
            var categoryId = 4;
            var category = new Category { Id = categoryId, Title = "ToDelete", Description = "ToDelete Desc" };

            _unitOfWorkMock.Setup(u => u.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _unitOfWorkMock.Setup(u => u.DeleteAsync(category)).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteCategoryAsync(categoryId);

            // Assert
            _unitOfWorkMock.Verify(u => u.DeleteAsync(category), Times.Once);
        }
    }
}
