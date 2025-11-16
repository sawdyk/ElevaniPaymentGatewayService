using ElevaniPaymentGateway.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElevaniPaymentGateway.Core.Entities
{
    [Table(nameof(MPGSTransaction))]
    public class MPGSTransaction : BaseEntity
    {
        public Guid TransactionId { get; set; }
        public string? BillingCountry { get; set; }
        public string? BillingAddress { get; set; }
        public string? BillingState { get; set; }
        public string? BillingCity { get; set; }
        public string? BillingZipCode { get; set; }
        public string? RedirectUrl { get; set; }

        public string? CardType { get; set; }
        public string? CardFirst6 { get; set; }
        public string? CardLast4 { get; set; }
        public string? ExpiryYear { get; set; }
        public string? ExpiryMonth { get; set; }
        public string? CardName { get; set; }
        public string? Token { get; set; }
        public string? AuthorizationUrl { get; set; }
        public string? AuthorizationMethod { get; set; }

        public bool IsVerified { get; set; } = false;
        public DateTime? DateVerified { get; set; } // date transaction was verified
        public DateTime? LastRetryDateTime { get; set; }
        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;


        [ForeignKey("TransactionId")]
        public Transaction Transaction { get; set; }
    }
}
