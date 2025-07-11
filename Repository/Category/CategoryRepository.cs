using DAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Category
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IGenericRepository<BusinessObject.Category> _repository;
        private readonly OrchidManagamentContext _context;

        public CategoryRepository(IGenericRepository<BusinessObject.Category> repository, OrchidManagamentContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IEnumerable<BusinessObject.Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.Orchids)
                .ToListAsync();
        }

        public async Task<BusinessObject.Category> GetCategoryByIdAsync(Guid id)
        {
            return await _context.Categories
                .Include(c => c.Orchids)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<bool> CreateCategoryAsync(BusinessObject.Category category)
        {
            // Check if category with same name already exists
            var exists = await _context.Categories.AnyAsync(c =>
                c.CategoryName.ToLower() == category.CategoryName.ToLower());

            if (exists)
                return false;

            _repository.Add(category);
            var result = await _repository.SaveChangesAsync();
            return result > 0;
        }
    }
}