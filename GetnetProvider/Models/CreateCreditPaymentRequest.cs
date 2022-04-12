using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{
    public class CreateCreditPaymentRequest
    {
        [JsonPropertyName("seller_id")]
        public string SellerId { get; set; }

        [Required]
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [StringLength(3)]
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [Required]
        [JsonPropertyName("order")]
        public PaymentRequestOrder Order { get; set; }

        [Required]
        [JsonPropertyName("customer")]
        public Customer Customer { get; set; }

        [JsonPropertyName("device")]
        public PaymentRequestDevice Device { get; set; }

        [JsonPropertyName("shippings")]
        public IEnumerable<Shipping> Shippings { get; set; }

        [JsonPropertyName("sub_merchant")]
        public SubMerchant SubMerchant { get; set; }

        [Required]
        [JsonPropertyName("credit")]
        public PaymentRequestCreditData Credit { get; set; }
    }

    public class PaymentRequestOrder
    {
        [Required]
        [JsonPropertyName("order_id")]
        [StringLength(36)]
        public string OrderId { get; set; }

        [JsonPropertyName("sales_tax")]
        public decimal SalesTax { get; set; }

        [JsonPropertyName("product_type")]
        public string ProductType { get; set; }
    }

    public class PaymentRequestDevice
    {
        [JsonPropertyName("ip_address")]
        public string IpAddress { get; set; }

        [JsonPropertyName("device_id")]
        public string DeviceId { get; set; }

        public PaymentRequestDevice() { }

        public PaymentRequestDevice(string ipAddress, string deviceId)
        {
            IpAddress = ipAddress;
            DeviceId = deviceId;
        }
    }

    public class PaymentRequestCreditData
    {
        [Required]
        [JsonPropertyName("delayed")]
        public bool Delayed { get; set; }

        //[JsonPropertyName("pre_authorization")]
        //public bool PreAuthorization { get; set; }

        [Required]
        [JsonPropertyName("save_card_data")]
        public bool SaveCardData { get; set; }

        [Required]
        [JsonPropertyName("transaction_type")]
        public string TransactionType { get; set; }

        [Required]
        [JsonPropertyName("number_installments")]
        public int NumberInstallments { get; set; }

        [JsonPropertyName("soft_descriptor")]
        [StringLength(22)]
        public string SoftDescriptor { get; set; }//"LOJA*TESTE*COMPRA-123"

        //[JsonPropertyName("dynamic_mcc")]
        //public int DynamicMcc { get; set; }

        [Required]
        [JsonPropertyName("card")]
        public PaymentRequestCard Card { get; set; }

        public PaymentRequestCreditData() { }

        public PaymentRequestCreditData(bool saveCardData, string transactionType, int numberInstallments, string softDescriptor, PaymentRequestCard card)
        {
            SaveCardData = saveCardData;
            TransactionType = transactionType;
            NumberInstallments = numberInstallments;
            SoftDescriptor = softDescriptor;
            Card = card;
        }
    }

    public class PaymentRequestCard
    {
        [Required]
        [JsonPropertyName("number_token")]
        [StringLength(128)]
        public string NumberToken { get; set; }

        [JsonPropertyName("cardholder_name")]
        [StringLength(26)]
        public string CardholderName { get; set; }

        [Required]
        [JsonPropertyName("security_code")]
        [StringLength(4, MinimumLength = 3)]
        public string SecurityCode { get; set; }

        [JsonPropertyName("brand")]
        [StringLength(50)]
        public string Brand { get; set; }

        [Required]
        [JsonPropertyName("expiration_month")]
        [StringLength(2)]
        public string ExpirationMonth { get; set; }

        [Required]
        [JsonPropertyName("expiration_year")]
        [StringLength(2)]
        public string ExpirationYear { get; set; }

        public PaymentRequestCard() { }

        public PaymentRequestCard(string numberToken, string cardholderName, string securityCode, string brand, string expirationMonth, string expirationYear)
        {
            NumberToken = numberToken;
            CardholderName = cardholderName;
            SecurityCode = securityCode;
            Brand = brand;
            ExpirationMonth = expirationMonth;
            ExpirationYear = expirationYear;
        }
    }
}