using ElevaniPaymentGateway.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElevaniPaymentGateway.Core.Entities
{
    [Table(nameof(Transaction))]
    public class Transaction : BaseEntity
    {
        public string MerchantId { get; set; }
        public string Reference { get; set; } //UUID - UNIQUE (generate unique transaction reference for each transactions)
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string? Narration { get; set; } //same as Description
        public string? CountryCode { get; set; }
        public string? CustomerFirstName { get; set; }
        public string? CustomerLastName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhoneNumber { get; set; }
        public PaymentGateways PaymentGateway { get; set; } // WemaMPGS, Gratip etc
        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
     

        [ForeignKey("MerchantId")]
        public Merchant Merchant { get; set; }

        public virtual GratipTransaction GratipTransaction { get; set; }

        //create two endpoints, one for collecting and processing card details (MPGs), the other, to generate a payment link (Payment gateways)
        //merchant initiate transaction, check the merchant payment gateway, if gratip, call the gratip service, if flutterwave, call the flutterwave service,
        //if WEMA_MPGS, call the WEMA_MPGS
        //persist data into each payment gateway table
    }
}
