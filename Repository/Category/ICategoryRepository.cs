
namespace Repository.Category
{
    public interface ICategoryRepository
    {
        Task<bool> CreateCategoryAsync(BusinessObject.Category category);
        Task<IEnumerable<BusinessObject.Category>> GetAllCategoriesAsync();
        Task<BusinessObject.Category> GetCategoryByIdAsync(Guid id);
    }
}