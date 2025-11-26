using ElevaniPaymentGateway.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElevaniPaymentGateway.Core.Entities
{
    [Table(nameof(GratipTransaction))]
    public class GratipTransaction : BaseEntity
    {
        public Guid TransactionId { get; set; }
        public string ExternalReference { get; set; }
        public string Currency { get; set; }
        public string Method { get; set; }
        public decimal Amount { get; set; }
        public string CountryCode { get; set; }
        public string? MerchantID { get; set; }

        public string? Description { get; set; }
        public string? CustomerFirstName { get; set; }
        public string? CustomerLastName { get; set; }
        public string? CustomerEmail { get; set; }

        public string? CollectionId { get; set; }
        public string? RequestReference { get; set; }
        public string? TransactionReference { get; set; }
        public string? PaymentURL { get; set; }
        public decimal SystemFee { get; set; }
        public decimal SystemFeePercentage { get; set; }
        public decimal ClientFee { get; set; }
        public decimal ClientFeePercentage { get; set; }
        public decimal TotalFees { get; set; }
        public decimal NetAmount { get; set; }
        //public string? Status { get; set; }

        public bool IsVerified { get; set; } = false;
        public DateTime? DateVerified { get; set; } // date transaction was verified
        public DateTime? LastRetryDateTime { get; set; }
        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;


        [ForeignKey("TransactionId")]
        public Transaction Transaction { get; set; }
    }
}
