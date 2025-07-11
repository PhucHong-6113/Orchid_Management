using BusinessObject;

namespace Repository.Authentication
{
    public interface ISystemAccountRepository
    {
        Task<Account> GetAccountByEmailAndPasswordAsync(string email, string password);
        Task<Account> GetAccountByIdAsync(Guid id);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<IEnumerable<Account>> SearchAccountsAsync(string nameOrEmail);
        Task<bool> UpdateAccountStatusAsync(Guid id, string status);
    }
}