using System.Text.Json.Serialization;

namespace Service.DTOs
{
    public class LoginModel
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
