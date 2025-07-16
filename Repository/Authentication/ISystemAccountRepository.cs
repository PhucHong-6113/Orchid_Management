    using BusinessObject;

namespace Repository.Authentication
{
    public interface ISystemAccountRepository
    {
        Task<Account> GetAccountByEmailAndPasswordAsync(string email, string password);
        Task<Account> GetAccountByIdAsync(Guid id);
        Task<Account> GetAccountByEmailAsync(string email);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<IEnumerable<Account>> SearchAccountsAsync(string nameOrEmail);
        Task<bool> UpdateAccountStatusAsync(Guid id, string status);
        Task<bool> UpdateAccountStatusAsync(string email, string status);
        Task<Account> CreateAccountAsync(Account account);
        Task<bool> EmailExistsAsync(string email);
    }
}