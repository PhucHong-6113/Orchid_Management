using BusinessObject;
using Repository.Authentication;
using Service.DTOs;
using System.Threading.Tasks;

namespace Service.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ISystemAccountRepository _accountRepository;

        public AuthenticationService(ISystemAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Account> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            return await _accountRepository.GetAccountByEmailAndPasswordAsync(email, password);
        }

        public async Task<Account> GetAccountByIdAsync(Guid id)
        {
            return await _accountRepository.GetAccountByIdAsync(id);
        }

        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            return await _accountRepository.GetAccountByEmailAsync(email);
        }

        public string GetRoleName(Guid? roleId)
        {
            if (roleId == null)
                return "guest";

            return roleId.Value.ToString() switch
            {
                "d5863bd5-1f21-4a32-9f71-f7f0925f7504" => "customer",
                "0a20fed9-b97a-46c7-8b96-ce582129de95" => "seller",
                "9dc5e7d2-dfb5-48f8-b45c-e925aa2062fc" => "admin",
                _ => "guest"
            };
        }

        public async Task<Account> RegisterAsync(RegisterModel registerModel)
        {
            // Check if email already exists
            if (await _accountRepository.EmailExistsAsync(registerModel.Email))
                return null;

            var account = new Account
            {
                AccountId = Guid.NewGuid(),
                AcountName = registerModel.Name,
                Email = registerModel.Email,
                Password = registerModel.Password, // In production, hash this password
                RoleId = GetCustomerRoleId(),
                Status = "Pending" // Set to pending until OTP verification
            };

            return await _accountRepository.CreateAccountAsync(account);
        }

        public async Task<bool> VerifyAccountAsync(string email)
        {
            return await _accountRepository.UpdateAccountStatusAsync(email, "Active");
        }

        public Guid GetCustomerRoleId()
        {
            return Guid.Parse("d5863bd5-1f21-4a32-9f71-f7f0925f7504"); // Customer role ID
        }
    }
}