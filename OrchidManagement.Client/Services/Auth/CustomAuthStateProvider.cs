using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OrchidManagement.Client.Services.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly LocalStorageService _localStorage;
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

        public CustomAuthStateProvider(LocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrEmpty(token))
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var tokenContent = _tokenHandler.ReadJwtToken(token);
            var claims = tokenContent.Claims.ToList();

            // Add name claim if not present
            var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (nameClaim == null)
            {
                var name = claims.FirstOrDefault(c => c.Type == "name")?.Value;
                if (!string.IsNullOrEmpty(name))
                    claims.Add(new Claim(ClaimTypes.Name, name));
            }

            // Add role claims
            var roles = claims.Where(c => c.Type == "role").ToList();
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Value));
            }

            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        public void NotifyUserAuthentication(string token)
        {
            var tokenContent = _tokenHandler.ReadJwtToken(token);
            var claims = tokenContent.Claims.ToList();
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);

            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}