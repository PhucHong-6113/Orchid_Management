using DAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Orchid
{
    public class OrchidRepository : IOrchidRepository
    {
        private readonly IGenericRepository<BusinessObject.Orchid> _repository;
        private readonly OrchidManagamentContext _context;

        public OrchidRepository(IGenericRepository<BusinessObject.Orchid> repository, OrchidManagamentContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IEnumerable<BusinessObject.Orchid>> GetAllOrchidsWithCategoriesAsync()
        {
            return await _context.Orchids
                .Include(o => o.Category)
                .ToListAsync();
        }

        public async Task<BusinessObject.Orchid> GetOrchidByIdAsync(Guid id)
        {
            return await _context.Orchids
                .Include(o => o.Category)
                .FirstOrDefaultAsync(o => o.OrchidId == id);
        }

        public async Task<bool> AddOrchidAsync(BusinessObject.Orchid orchid)
        {
            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == orchid.CategoryId);
            if (!categoryExists)
                return false;

            _repository.Add(orchid);
            var result = await _repository.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateOrchidAsync(BusinessObject.Orchid orchid)
        {
            var existingOrchid = await _context.Orchids.FindAsync(orchid.OrchidId);
            if (existingOrchid == null)
                return false;
                
            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == orchid.CategoryId);
            if (!categoryExists)
                return false;
                
            existingOrchid.OrchidName = orchid.OrchidName;
            existingOrchid.OrchidDescription = orchid.OrchidDescription;
            existingOrchid.OrchidUrl = orchid.OrchidUrl;
            existingOrchid.Price = orchid.Price;
            existingOrchid.CategoryId = orchid.CategoryId;
            existingOrchid.IsNatural = orchid.IsNatural;
            
            _context.Orchids.Update(existingOrchid);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<BusinessObject.Orchid>> SearchOrchidsAsync(string? name, Guid? categoryId, bool? isNatural, decimal? minPrice, decimal? maxPrice, int page, int pageSize)
        {
            var query = _context.Orchids.Include(o => o.Category).AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(o => o.OrchidName.Contains(name));
            }

            if (categoryId.HasValue && categoryId != Guid.Empty)
            {
                query = query.Where(o => o.CategoryId == categoryId);
            }

            if (isNatural.HasValue)
            {
                query = query.Where(o => o.IsNatural == isNatural);
            }

            // Apply price range filter
            if (minPrice.HasValue)
            {
                query = query.Where(o => o.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(o => o.Price <= maxPrice.Value);
            }

            // Default ordering by name
            query = query.OrderBy(o => o.OrchidName);

            // Apply pagination
            var skip = (page - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);

            return await query.ToListAsync();
        }
    }
}