using Microsoft.AspNetCore.Mvc;
using Service.Authentication;
using Service.DTOs;
using Service.JWT;

namespace PRN231_SE172426_Exercise23.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IJwtTokenService jwtTokenService, IAuthenticationService authService)
        {
            _jwtTokenService = jwtTokenService;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            Console.WriteLine($"Login endpoint hit with email: {model?.Email}");
            
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return BadRequest(new { message = "Email and password are required" });

            var user = await _authService.AuthenticateAsync(model.Email, model.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid email or password" });

            string roleName = _authService.GetRoleName(user.RoleId);

            var token = _jwtTokenService.GenerateToken(
                userId: user.AccountId.ToString(),
                username: user.AcountName ?? user.Email,
                roles: new[] { roleName }
            );

            return Ok(new
            {
                token,
                role = roleName,
                userId = user.AccountId,
                username = user.AcountName ?? user.Email
            });
        }
    }
}