using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{
    public class ReadCustomerResponse : Customer
    {
        [JsonPropertyName("seller_id")]
        public string SellerId { get; set; }

        [JsonPropertyName("observation")]
        public string? Observation { get; set; }
    }
}