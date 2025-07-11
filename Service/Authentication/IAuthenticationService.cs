using BusinessObject;

namespace Service.Authentication
{
    public interface IAuthenticationService
    {
        Task<Account> AuthenticateAsync(string email, string password);
        Task<Account> GetAccountByIdAsync(Guid id);
        string GetRoleName(Guid? roleId);
    }
}