using OrchidManagement.Client.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace OrchidManagement.Client.Services.Account
{
    public class AccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<AccountDto>>("api/admin/accounts");
        }

        public async Task<IEnumerable<AccountDto>> SearchAccountsAsync(string nameOrEmail)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<AccountDto>>($"api/admin/accounts/search?query={nameOrEmail}");
        }

        public async Task<AccountDto> GetAccountByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<AccountDto>($"api/admin/accounts/{id}");
        }

        public async Task<bool> ToggleAccountStatusAsync(Guid id)
        {
            var response = await _httpClient.PutAsync($"api/admin/accounts/{id}/toggle-status", null);
            return response.IsSuccessStatusCode;
        }
    }
}