using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Service.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<bool> SendOTPEmailAsync(string toEmail, string otp, string userName)
        {
            string subject = "Verify Your Account - OTP Code";
            string body = $@"
                <html>
                <body>
                    <h2>Welcome to Orchid Management System!</h2>
                    <p>Dear {userName},</p>
                    <p>Thank you for registering with us. To complete your registration, please use the following OTP code:</p>
                    <h3 style='color: #007bff; font-size: 24px; text-align: center; padding: 20px; border: 2px solid #007bff; display: inline-block;'>{otp}</h3>
                    <p>This OTP code will expire in 10 minutes.</p>
                    <p>If you did not request this registration, please ignore this email.</p>
                    <br>
                    <p>Best regards,<br>Orchid Management Team</p>
                </body>
                </html>";

            return await SendEmailAsync(toEmail, subject, body);
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using var client = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort);
                client.EnableSsl = _emailSettings.EnableSsl;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);

                using var message = new MailMessage();
                message.From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName);
                message.To.Add(toEmail);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                await client.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (you might want to use ILogger here)
                Console.WriteLine($"Email sending failed: {ex.Message}");
                return false;
            }
        }
    }
    public class EmailSettings
    {
        public string SmtpHost { get; set; } = null!;
        public int SmtpPort { get; set; }
        public bool EnableSsl { get; set; }
        public string SmtpUsername { get; set; } = null!;
        public string SmtpPassword { get; set; } = null!;
        public string FromEmail { get; set; } = null!;
        public string FromName { get; set; } = null!;
    }
}