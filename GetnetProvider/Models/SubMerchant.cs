using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{
    public class SubMerchant
    {
        [JsonPropertyName("identification_code")]
        [StringLength(15)]
        public string IdentificationCode { get; set; }

        [JsonPropertyName("document_type")]
        [StringLength(4, MinimumLength = 3)]
        public string DocumentType { get; set; }

        [JsonPropertyName("document_number")]
        [StringLength(14, MinimumLength = 11)]
        public string DocumentNumber { get; set; }

        [JsonPropertyName("address")]
        [StringLength(40)]
        public string Address { get; set; }

        [JsonPropertyName("city")]
        [StringLength(13)]
        public string City { get; set; }

        [JsonPropertyName("state")]
        [StringLength(2)]
        public string State { get; set; }//UF

        [JsonPropertyName("postal_code")]
        [StringLength(8)]
        public string PostalCode { get; set; }
    }           
}