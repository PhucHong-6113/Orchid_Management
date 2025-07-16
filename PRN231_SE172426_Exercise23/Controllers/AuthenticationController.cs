using Microsoft.AspNetCore.Mvc;
using Service.Authentication;
using Service.DTOs;
using Service.EmailService;
using Service.JWT;

namespace PRN231_SE172426_Exercise23.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IAuthenticationService _authService;
        private readonly IEmailService _emailService;
        private readonly IOTPService _otpService;

        public AuthenticationController(
            IJwtTokenService jwtTokenService, 
            IAuthenticationService authService,
            IEmailService emailService,
            IOTPService otpService)
        {
            _jwtTokenService = jwtTokenService;
            _authService = authService;
            _emailService = emailService;
            _otpService = otpService;
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
                email: user.Email,
                roles: new[] { roleName }
            );

            var jwtPayload = new JwtPayload
            {
                Token = token,
                Role = roleName,
                UserId = user.AccountId,
                Username = user.AcountName ?? user.Email,
                Email = user.Email
            };

            return Ok(jwtPayload);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newUser = await _authService.RegisterAsync(model);
            
            if (newUser == null)
                return BadRequest(new { message = "Email already exists or registration failed" });

            // Generate and send OTP
            var otp = _otpService.GenerateOTP();
            await _otpService.StoreOTPAsync(model.Email, otp);

            var emailSent = await _emailService.SendOTPEmailAsync(model.Email, otp, model.Name);
            
            if (!emailSent)
            {
                return BadRequest(new { message = "Registration successful but failed to send verification email. Please contact support." });
            }

            return Ok(new { 
                message = "Registration successful. Please check your email for OTP verification.",
                email = model.Email
            });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP(VerifyOTPModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verify OTP
            var isValidOTP = await _otpService.VerifyOTPAsync(model.Email, model.OTP);
            
            if (!isValidOTP)
                return BadRequest(new { message = "Invalid or expired OTP" });

            // Activate account
            var activated = await _authService.VerifyAccountAsync(model.Email);
            
            if (!activated)
                return BadRequest(new { message = "Failed to activate account" });

            // Delete OTP after successful verification
            await _otpService.DeleteOTPAsync(model.Email);

            // Get activated user and generate token
            var user = await _authService.GetAccountByEmailAsync(model.Email);
            string roleName = _authService.GetRoleName(user.RoleId);

            var token = _jwtTokenService.GenerateToken(
                userId: user.AccountId.ToString(),
                username: user.AcountName ?? user.Email,
                email: user.Email,
                roles: new[] { roleName }
            );

            var jwtPayload = new JwtPayload
            {
                Token = token,
                Role = roleName,
                UserId = user.AccountId,
                Username = user.AcountName ?? user.Email,
                Email = user.Email
            };

            return Ok(jwtPayload);
        }

        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOTP([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest(new { message = "Email is required" });

            var user = await _authService.GetAccountByEmailAsync(email);
            
            if (user == null)
                return BadRequest(new { message = "User not found" });
                
            if (user.Status == "Active")
                return BadRequest(new { message = "Account is already verified" });

            // Generate and send new OTP
            var otp = _otpService.GenerateOTP();
            await _otpService.StoreOTPAsync(email, otp);

            var emailSent = await _emailService.SendOTPEmailAsync(email, otp, user.AcountName);
            
            if (!emailSent)
                return BadRequest(new { message = "Failed to send verification email" });

            return Ok(new { message = "OTP sent successfully to your email" });
        }
    }
}