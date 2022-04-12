using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{
    public class CardVerificationResponse
    {
        [JsonPropertyName("verification_id")]
        public string VerificationId { get; set; }

        [JsonPropertyName("authorization_code")]
        public string AuthorizationCode { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }   
}