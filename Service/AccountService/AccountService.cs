
using Repository.Authentication;
using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Admin
{
    public class AccountService : IAccountService
    {
        private readonly ISystemAccountRepository _accountRepository;

        public AccountService(ISystemAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAccountsAsync();
            return accounts.Select(MapAccountToDto);
        }

        public async Task<IEnumerable<AccountDto>> SearchAccountsAsync(string nameOrEmail)
        {
            var accounts = await _accountRepository.SearchAccountsAsync(nameOrEmail);
            return accounts.Select(MapAccountToDto);
        }

        public async Task<AccountDto> GetAccountByIdAsync(Guid id)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id);
            return account != null ? MapAccountToDto(account) : null;
        }

        public async Task<bool> ToggleAccountStatusAsync(Guid id)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id);
            if (account == null)
                return false;

            // Toggle status
            string newStatus = account.Status == "Active" ? "Inactive" : "Active";

            return await _accountRepository.UpdateAccountStatusAsync(id, newStatus);
        }

        private AccountDto MapAccountToDto(BusinessObject.Account account)
        {
            return new AccountDto
            {
                AccountId = account.AccountId,
                AccountName = account.AcountName,
                Email = account.Email,
                RoleName = account.Role?.RoleName,
                RoleId = account.RoleId,
                Status = account.Status ?? "Active" // Default to Active if null
            };
        }
    }
}