using ElevaniPaymentGateway.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElevaniPaymentGateway.Core.Entities
{
    [Table(nameof(PayAgencyTransaction))]
    public class PayAgencyTransaction : BaseEntity
    {
        public Guid TransactionId { get; set; }
        public string Reference { get; set; } //merchant reference - order_id
        public string TransactionReference { get; set; } //pay agency transaction reference - transaction_id
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? IPAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; } //Narration
        public string Currency { get; set; }
        public string CardNumber { get; set; }
        public string? CardExpiryMonth { get; set; }
        public string? CardExpiryYear { get; set; }
        public string? CardCVV { get; set; }
        public string RedirectUrl { get; set; }
        public string? OTPRedirectUrl { get; set; }
        public string? WebHookUrl { get; set; }
        public string? Message { get; set; }

        public bool IsVerified { get; set; } = false;
        public DateTime? DateVerified { get; set; } // date transaction was verified
        public DateTime? LastRetryDateTime { get; set; }
        public TransactionStatus Status { get; set; }


        [ForeignKey("TransactionId")]
        public Transaction Transaction { get; set; }
    }
}
