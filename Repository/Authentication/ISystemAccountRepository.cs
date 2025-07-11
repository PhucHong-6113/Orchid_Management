using BusinessObject;

namespace Repository.Authentication
{
    public interface ISystemAccountRepository
    {
        Task<Account> GetAccountByEmailAndPasswordAsync(string email, string password);
        Task<Account> GetAccountByIdAsync(Guid id);
    }
}