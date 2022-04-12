using GetnetProvider.Models;

namespace API.Models.CommandRequests
{
    public class PaymentCommandRequest
    {
        public string SoftDescriptor { get; set; }
        public bool SaveCardData { get; set; }
        public int NumberInstallments { get; set; }
        public int Amount { get; set; }
        public Customer Customer { get; set; }
        public string CardNumber { get; set; }
        public string CardholderName { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string SecurityCode { get; set; }
        public PaymentRequestOrder Order { get; set; }
    }
}
