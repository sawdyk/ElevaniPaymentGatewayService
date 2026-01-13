using ElevaniPaymentGateway.Core.Enums;

namespace ElevaniPaymentGateway.Core.Models.Dto
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public string? MerchantId { get; set; }
        public string? Reference { get; set; }
        public string? Currency { get; set; }
        public decimal Amount { get; set; }
        public string? Narration { get; set; }
        public string? CountryCode { get; set; }
        public string? CustomerFirstName { get; set; }
        public string? CustomerLastName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhoneNumber { get; set; }
        public PaymentGateways PaymentGateway { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public GratipTransactionDto GratipTransaction { get; set; }
    }
}
