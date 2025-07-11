using Service.DTOs;

namespace Service.Admin
{
    public interface IAccountService
    {
        Task<AccountDto> GetAccountByIdAsync(Guid id);
        Task<IEnumerable<AccountDto>> GetAllAccountsAsync();
        Task<IEnumerable<AccountDto>> SearchAccountsAsync(string nameOrEmail);
        Task<bool> ToggleAccountStatusAsync(Guid id);
    }
}