using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{
    public abstract class CreateCustomerCommandBase
    {
        [Required]
        [StringLength(36)]
        [JsonPropertyName("seller_id")]
        public string SellerId { get; set; }

        [StringLength(100)]
        [JsonPropertyName("customer_id")]
        public string? CustomerId { get; set; }

        [Required]
        [StringLength(40)]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(80)]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(10)]
        [JsonPropertyName("document_type")]
        public string DocumentType { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 11)]
        [JsonPropertyName("document_number")]
        public string DocumentNumber { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [JsonPropertyName("birth_date")]//YYYY-MM-DD
        public string? BirthDate { get; set; }

        [StringLength(15)]
        [JsonPropertyName("phone_number")]
        public string? PhoneNumber { get; set; }

        [StringLength(15)]
        [JsonPropertyName("celphone_number")]
        public string? CelphoneNumber { get; set; }

        [EmailAddress]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("observation")]
        public string? Observation { get; set; }

        [JsonPropertyName("address")]
        public Address? Address { get; set; }
    }


    public class CreateCustomerRequest : CreateCustomerCommandBase
    {
        protected CreateCustomerRequest(string sellerId, string? customerId, string firstName, string lastName, string documentType, string documentNumber, string? birthDate, string? phoneNumber, string? celphoneNumber, string? email, string? observation, Address? address)
        {
            SellerId = sellerId;
            FirstName = firstName;
            LastName = lastName;
            DocumentType = documentType;
            DocumentNumber = documentNumber;
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
            CelphoneNumber = celphoneNumber;
            Email = email;
            Observation = observation;
            Address = address;
        }

        public CreateCustomerRequest(string sellerId, string observation, Customer customer)
        {
            SellerId = sellerId;
            FirstName = customer.Firstname;
            LastName = customer.Lastname;
            DocumentType = customer.DocumentType.ToString();
            DocumentNumber = customer.DocumentNumber;
            BirthDate = customer.BirthDate;
            PhoneNumber = customer.Phone;
            CelphoneNumber = customer.CelPhone;
            Email = customer.Email;
            Observation = observation;
            Address = customer.Address;
        }

        public CreateCustomerRequest(string sellerId, Customer customer)
        {
            SellerId = sellerId;
            FirstName = customer.Firstname;
            LastName = customer.Lastname;
            DocumentType = customer.DocumentType.ToString();
            DocumentNumber = customer.DocumentNumber;
            BirthDate = customer.BirthDate;
            PhoneNumber = customer.Phone;
            CelphoneNumber = customer.CelPhone;
            Email = customer.Email;
            Address = customer.Address;
        }
    }

    public class CreateCustomerResponse : CreateCustomerCommandBase
    {
        [StringLength(20)]
        [JsonPropertyName("status")]
        public string Status { get; set; }

        public CreateCustomerResponse(string sellerId, string? customerId, string firstName, string lastName, string documentType, string documentNumber, string? birthDate, string? phoneNumber, string? celphoneNumber, string? email, string? observation, Address? address, string status)
        {
            SellerId = sellerId;
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            DocumentType = documentType;
            DocumentNumber = documentNumber;
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
            CelphoneNumber = celphoneNumber;
            Email = email;
            Observation = observation;
            Address = address;
            Status = status;
        }
    }
}