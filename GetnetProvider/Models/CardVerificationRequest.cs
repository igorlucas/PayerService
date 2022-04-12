using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{
    public class CardVerificationRequest
    {
        [Required]
        [JsonPropertyName("number_token")]
        public string NumberToken { get; set; }

        [StringLength(50)]
        [JsonPropertyName("brand")]
        public string Brand { get; set; }

        [StringLength(26)]
        [JsonPropertyName("cardholder_name")]
        public string CardholderName { get; set; }

        [Required]
        [StringLength(2)]
        [JsonPropertyName("expiration_month")]
        public string ExpirationMonth { get; set; }

        [Required]
        [StringLength(2)]
        [JsonPropertyName("expiration_year")]
        public string ExpirationYear { get; set; }

        [Required]
        [StringLength(2)]
        [JsonPropertyName("security_code")]
        public string SecurityCode { get; set; }

        public CardVerificationRequest() { }
        public CardVerificationRequest(string numberToken, string brand, string cardholderName, string expirationMonth, string expirationYear, string securityCode)
        {
            NumberToken = numberToken;
            Brand = brand;
            CardholderName = cardholderName;
            ExpirationMonth = expirationMonth;
            ExpirationYear = expirationYear;
            SecurityCode = securityCode;
        }
    }
}