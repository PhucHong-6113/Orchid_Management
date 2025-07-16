using System.Threading.Tasks;

namespace Service.EmailService
{
    public interface IEmailService
    {
        Task<bool> SendOTPEmailAsync(string toEmail, string otp, string userName);
        Task<bool> SendEmailAsync(string toEmail, string subject, string body);
    }
}