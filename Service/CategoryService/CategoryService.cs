using Repository.Category;
using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();

            return categories.Select(c => new CategoryDto
            {
                Id = c.CategoryId,
                Name = c.CategoryName,
                OrchidCount = c.Orchids?.Count ?? 0
            });
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
                return null;

            return new CategoryDto
            {
                Id = category.CategoryId,
                Name = category.CategoryName,
                OrchidCount = category.Orchids?.Count ?? 0
            };
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto categoryDto)
        {
            var category = new BusinessObject.Category
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = categoryDto.CategoryName,
                Orchids = new List<BusinessObject.Orchid>()
            };

            var success = await _categoryRepository.CreateCategoryAsync(category);
            if (!success)
                return null;

            return new CategoryDto
            {
                Id = category.CategoryId,
                Name = category.CategoryName,
                OrchidCount = 0
            };
        }
    }
}