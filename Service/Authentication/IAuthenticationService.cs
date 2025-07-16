using BusinessObject;
using Service.DTOs;

namespace Service.Authentication
{
    public interface IAuthenticationService
    {
        Task<Account> AuthenticateAsync(string email, string password);
        Task<Account> GetAccountByIdAsync(Guid id);
        string GetRoleName(Guid? roleId);
        Task<Account> RegisterAsync(RegisterModel registerModel);
        Task<bool> VerifyAccountAsync(string email);
        Task<Account> GetAccountByEmailAsync(string email);
        Guid GetCustomerRoleId();
    }
}