using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{
    public class Customer
    {
        [Required]
        [JsonPropertyName("customer_id")]
        public string Id { get; set; }

        [Required]
        [JsonPropertyName("name")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [JsonPropertyName("first_name")]
        [StringLength(40)]
        public string Firstname { get; set; }

        [Required]
        [JsonPropertyName("last_name")]
        [StringLength(80)]
        public string Lastname { get; set; }

        [Required]
        [JsonPropertyName("document_type")]
        public string DocumentType { get; set; }

        [Required]
        [JsonPropertyName("document_number")]
        [StringLength(15, MinimumLength = 11)]
        public string DocumentNumber { get; set; }

        [JsonPropertyName("phone_number")]
        [Phone]
        public string? Phone { get; set; }

        [JsonPropertyName("celphone_number")]
        [Phone]
        public string? CelPhone { get; set; }

        [JsonPropertyName("email")]
        [EmailAddress]
        public string? Email { get; set; }

        [JsonPropertyName("birth_date")]//YYYY-MM-DD
        [StringLength(10, MinimumLength = 10)]
        public string? BirthDate { get; set; }

        [JsonPropertyName("address")]
        public Address? Address { get; set; }

        [Required]
        [JsonPropertyName("billing_address")]
        public Address BillingAddress { get; set; }
            
        public Customer() { }
        public Customer(string firstname, string lastname, string documentType, string documentNumber, string? phone, string? celPhone, string? email, string? birthDate, Address? address)
        {
            Firstname = firstname;
            Lastname = lastname;
            DocumentType = documentType;
            DocumentNumber = documentNumber;
            Phone = phone;
            CelPhone = celPhone;
            Email = email;
            BirthDate = birthDate;
            Address = address;
        }
    }
}