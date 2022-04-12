using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{
    public class CreateCardTokenRequest
    {
        [Required]
        [JsonPropertyName("card_number")]
        [StringLength(19, MinimumLength = 13)]
        public string CardNumber { get; set; }

        [JsonPropertyName("customer_id")]
        [StringLength(100)]
        public string CustomerId { get; set; }

        public CreateCardTokenRequest() { }

        public CreateCardTokenRequest(string cardNumber, string customerId)
        {
            CardNumber = cardNumber;
            CustomerId = customerId;
        }

        public override string ToString() => $"cardNumber: {GetMaskCardNumber()}; customerId: {this.CustomerId}";

        private string GetMaskCardNumber()
        {
            var lengthCardNumber = this.CardNumber.Length;
            var lastFourNumbers = this.CardNumber.Substring(lengthCardNumber - 4);
            var cardNumber = lastFourNumbers.PadLeft(lengthCardNumber - 4, '*');
            return cardNumber;
        }
    }
}