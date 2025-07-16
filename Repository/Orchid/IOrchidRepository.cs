using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Orchid
{
    public interface IOrchidRepository
    {
        Task<IEnumerable<BusinessObject.Orchid>> GetAllOrchidsWithCategoriesAsync();
        Task<BusinessObject.Orchid> GetOrchidByIdAsync(Guid id);
        Task<bool> AddOrchidAsync(BusinessObject.Orchid orchid);
        Task<bool> UpdateOrchidAsync(BusinessObject.Orchid orchid);
        Task<IEnumerable<BusinessObject.Orchid>> SearchOrchidsAsync(string? name, Guid? categoryId, bool? isNatural, decimal? minPrice, decimal? maxPrice, int page, int pageSize);
    }
}