using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{
    public class Address
    {
        [StringLength(60)]
        [JsonPropertyName("street")]
        public string Street { get; set; }

        [StringLength(10)]
        [JsonPropertyName("number")]
        public string Number { get; set; }

        [StringLength(60)]
        [JsonPropertyName("complement")]
        public string Complement { get; set; }

        [StringLength(40)]
        [JsonPropertyName("district")]
        public string District { get; set; }

        [StringLength(40)]
        [JsonPropertyName("city")]
        public string City { get; set; }

        [StringLength(20)]
        [JsonPropertyName("state")]
        public string State { get; set; }

        [StringLength(20)]
        [JsonPropertyName("country")]
        public string Country { get; set; }

        [StringLength(8)]
        [JsonPropertyName("postal_code")]
        public string PostalCode { get; set; }

        public Address() { }

        public Address(string street, string number, string complement, string district, string city, string state, string country, string postalCode)
        {
            Street = street;
            Number = number;
            Complement = complement;
            District = district;
            City = city;
            State = state;
            Country = country;
            PostalCode = postalCode;
        }
    }
}