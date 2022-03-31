using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{
    public class CreateAccessTokenResponse
    {
        [JsonPropertyName("access_token")]
        [StringLength(36)]
        public string Token { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        public CreateAccessTokenErrorResponse? ErrorResponse { get; set; }
    }

    public class CreateAccessTokenErrorResponse
    {
        public string Error { get; set; }

        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; }
    }
}
