using BusinessObject;
using DAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    }
}