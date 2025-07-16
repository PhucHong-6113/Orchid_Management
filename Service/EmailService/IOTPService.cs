namespace Service.EmailService
{
    public interface IOTPService
    {
        Task<bool> DeleteOTPAsync(string email);
        string GenerateOTP();
        Task<bool> StoreOTPAsync(string email, string otp);
        Task<bool> VerifyOTPAsync(string email, string otp);
    }
}