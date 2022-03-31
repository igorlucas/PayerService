using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{
    public class ListCustomerResponse
    {
        [JsonPropertyName("customers")]
        public IEnumerable<Customer> Customers { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}
