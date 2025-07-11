using System;
using System.Text.Json.Serialization;

namespace OrchidManagement.Client.Models
{
    public class AccountDto
    {
        [JsonPropertyName("account_id")]
        public Guid AccountId { get; set; }

        [JsonPropertyName("account_name")]
        public string AccountName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("role_name")]
        public string RoleName { get; set; }

        [JsonPropertyName("role_id")]
        public Guid RoleId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class LoginModel
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}