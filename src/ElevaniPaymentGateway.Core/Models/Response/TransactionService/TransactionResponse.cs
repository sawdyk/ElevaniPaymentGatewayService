using ElevaniPaymentGateway.Core.Enums;

namespace ElevaniPaymentGateway.Core.Models.Response.TransactionService
{
    public class TransactionResponse
    {
        public string MerchantID { get; set; }
        public string TransactionReference { get; set; }
        public string PaymentUrl { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CountryCode { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
