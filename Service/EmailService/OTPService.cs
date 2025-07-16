using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Service.EmailService
{
    public class OTPService : IOTPService
    {
        private readonly IMemoryCache _cache;
        private const int OTP_EXPIRY_MINUTES = 10;

        public OTPService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public Task<bool> StoreOTPAsync(string email, string otp)
        {
            try
            {
                var key = $"otp_{email}";
                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OTP_EXPIRY_MINUTES)
                };

                _cache.Set(key, otp, options);
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public Task<bool> VerifyOTPAsync(string email, string otp)
        {
            try
            {
                var key = $"otp_{email}";
                if (_cache.TryGetValue(key, out string? storedOtp))
                {
                    return Task.FromResult(storedOtp == otp);
                }
                return Task.FromResult(false);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public Task<bool> DeleteOTPAsync(string email)
        {
            try
            {
                var key = $"otp_{email}";
                _cache.Remove(key);
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}