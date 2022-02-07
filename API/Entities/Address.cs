using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Address
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(60)]
        public string Street { get; set; }

        [Required]
        [StringLength(10)]
        public string Number { get; set; }

        [StringLength(60)]
        public string Complement { get; set; }

        [Required]
        [StringLength(40)]
        public string District { get; set; }

        [Required]
        [StringLength(40)]
        public string City { get; set; }

        [Required]
        [StringLength(20)]
        public string State { get; set; }

        [Required]
        [StringLength(20)]
        public string Country { get; set; }

        [Required]
        [StringLength(8)]
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
