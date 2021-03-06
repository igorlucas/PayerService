using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Customer
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(120)]
        public string FullName { get; set; }

        [Required]//CPF-CNPJ
        public string DocumentType { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 11)]
        public string DocumentValue { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? CellPhone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required]
        public Address Address { get; set; }

        public Customer() { }
        public Customer(string fullName, string documentType, string documentValue, DateTime? birthDate, string? phone, string? cellPhone, string? email, Address address)
        {
            FullName = fullName;
            DocumentType = documentType;
            DocumentValue = documentValue;
            BirthDate = birthDate;
            Phone = phone;
            CellPhone = cellPhone;
            Email = email;
            Address = address;
        }
    }
}