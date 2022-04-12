using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{
    public class CreateCreditPaymentResponse
    {
        [JsonPropertyName("payment_id")]
        public string PaymentId { get; set; }

        [JsonPropertyName("seller_id")]
        public string SellerId { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("order_id")]
        public string OrderId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("received_at")]
        public string ReceivedAt { get; set; }

        [JsonPropertyName("credit")]    
        public CreditTransactionData Credit { get; set; }
    }

    public class CreditTransactionData  
    {
        [JsonPropertyName("delayed")]
        public bool Delayed { get; set; }

        [JsonPropertyName("authorization_code")]
        public string AuthorizationCode { get; set; }

        [JsonPropertyName("authorized_at")]
        public string AuthorizedAt { get; set; }

        [JsonPropertyName("reason_code")]
        public string ReasonCode { get; set; }

        [JsonPropertyName("reason_message")]
        public string ReasonMessage { get; set; }

        [JsonPropertyName("acquirer")]
        public string Acquirer { get; set; }

        [JsonPropertyName("soft_descriptor")]
        public string SoftDescriptor { get; set; }

        [JsonPropertyName("brand")]
        public string Brand { get; set; }

        [JsonPropertyName("terminal_nsu")]
        public string TerminalNSU { get; set; }

        [JsonPropertyName("acquirer_transaction_id")]
        public string AcquirerTransactionId { get; set; }

        [JsonPropertyName("transaction_id")]
        public string TransactionId { get; set; }

        [JsonPropertyName("first_installment_amount")]
        public string FirstInstallmentAmount { get; set; }

        [JsonPropertyName("other_installment_amount")]
        public string OtherInstallmentAmount { get; set; }

        [JsonPropertyName("total_installment_amount")]
        public string TotalInstallmentAmount { get; set; }
    }                           
}