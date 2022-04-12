using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{
    public class Shipping
    {
        [JsonPropertyName("first_name")]
        [StringLength(40)]
        public string FirstName { get; set; }

        [JsonPropertyName("name")]
        [StringLength(100)]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        [EmailAddress]
        public string Email { get; set; }

        [JsonPropertyName("phone_number")]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("shipping_amount")]
        public int ShippingAmount { get; set; }

        [JsonPropertyName("address")]
        public Address Address { get; set; }
    }       
}