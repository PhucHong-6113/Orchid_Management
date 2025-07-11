using System.Text.Json.Serialization;

namespace OrchidManagement.Client.Models
{
    public class AuthorizationDto
    {
        public class LoginModel
        {
            [JsonPropertyName("email")]
            public string Email { get; set; }

            [JsonPropertyName("password")]
            public string Password { get; set; }
        }

        public class LoginResult
        {
            [JsonPropertyName("token")]
            public string Token { get; set; }

            [JsonPropertyName("role")]
            public string Role { get; set; }

            [JsonPropertyName("userId")]
            public Guid UserId { get; set; }

            [JsonPropertyName("username")]
            public string Username { get; set; }
        }
    }
}
