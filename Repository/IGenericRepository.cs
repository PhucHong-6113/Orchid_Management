using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository
{
    public interface IGenericRepository<T> where T : class
    {
        // Read operations
        IEnumerable<T> GetAll();
        T GetById(object id);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        
        // Create operations
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        
        // Update operations
        void Update(T entity);
        
        // Delete operations
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        
        // Save changes
        int SaveChanges();
        
        // Async versions
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(object id);
        Task<int> SaveChangesAsync();
    }
}
