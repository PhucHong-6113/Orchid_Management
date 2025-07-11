using BusinessObject;
using DAO;
using Microsoft.EntityFrameworkCore;
using Repository;
using System.Threading.Tasks;

namespace Repository.Authentication
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        private readonly IGenericRepository<Account> _repository;

        public SystemAccountRepository(IGenericRepository<Account> repository)
        {
            _repository = repository;
        }

        public async Task<Account> GetAccountByEmailAndPasswordAsync(string email, string password)
        {
            // No hash
            var accounts = _repository.Find(a => a.Email == email && a.Password == password);
            return accounts.FirstOrDefault();
        }

        public async Task<Account> GetAccountByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}