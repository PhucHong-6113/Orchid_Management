using Microsoft.AspNetCore.Components.Authorization;
using OrchidManagement.Client.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using static OrchidManagement.Client.Models.AuthorizationDto;

namespace OrchidManagement.Client.Services.Auth
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly LocalStorageService _localStorage;

        public AuthService(HttpClient httpClient,
                          AuthenticationStateProvider authStateProvider,
                          LocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<LoginResult> Login(Models.LoginModel loginModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);

            if (response.IsSuccessStatusCode)
            {
                var loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();
                await _localStorage.SetItemAsync("authToken", loginResult.Token);

                ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(loginResult.Token);

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("bearer", loginResult.Token);

                return loginResult;
            }

            return null;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}