using BusinessObject;
using DAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Authentication
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        private readonly IGenericRepository<Account> _repository;
        private readonly OrchidManagamentContext _context;

        public SystemAccountRepository(IGenericRepository<Account> repository, OrchidManagamentContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<Account> GetAccountByEmailAndPasswordAsync(string email, string password)
        {
            // Normally you'd hash the password before comparing
            var accounts = _repository.Find(a => a.Email == email && 
                                              a.Password == password &&
                                              a.Status == "Active");
            return accounts.FirstOrDefault();
        }

        public async Task<Account> GetAccountByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts
                .Include(a => a.Role)
                .ToListAsync();
        }

        public async Task<IEnumerable<Account>> SearchAccountsAsync(string nameOrEmail)
        {
            if (string.IsNullOrWhiteSpace(nameOrEmail))
                return await GetAllAccountsAsync();

            return await _context.Accounts
                .Include(a => a.Role)
                .Where(a => a.AcountName.Contains(nameOrEmail) || a.Email.Contains(nameOrEmail))
                .ToListAsync();
        }

        public async Task<bool> UpdateAccountStatusAsync(Guid id, string status)
        {
            if (status != "Active" && status != "Inactive")
                return false;
                
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
                return false;

            account.Status = status;
            _context.Accounts.Update(account);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}