namespace Repository.Orchid
{
    public interface IOrchidRepository
    {
        Task<IEnumerable<BusinessObject.Orchid>> GetAllOrchidsWithCategoriesAsync();
        Task<BusinessObject.Orchid> GetOrchidByIdAsync(Guid id);
        Task<bool> AddOrchidAsync(BusinessObject.Orchid orchid);
        Task<bool> UpdateOrchidAsync(BusinessObject.Orchid orchid);
    }
}