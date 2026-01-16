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
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? IPAddress { get; set; }
        public string? CardNumber { get; set; }
        public string? CardExpiryMonth { get; set; }
        public string? CardExpiryYear { get; set; }
        public string? CardCVV { get; set; }
        public string? RedirectUrl { get; set; }
        public string? WebHookUrl { get; set; }

        public PaymentGateways PaymentGateway { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public GratipTransactionDto GratipTransaction { get; set; }
        public PayAgencyTransactionDto PayAgencyTransaction{ get; set; }
    }
}
