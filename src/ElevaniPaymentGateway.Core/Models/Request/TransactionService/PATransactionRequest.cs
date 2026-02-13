using System.ComponentModel.DataAnnotations;

namespace ElevaniPaymentGateway.Core.Models.Request.TransactionService
{
    public class PATransactionRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        //public string IPAddress { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiryMonth { get; set; }
        public string CardExpiryYear { get; set; }
        public string CardCVV { get; set; }
        //public string RedirectUrl { get; set; }
        public string Reference { get; set; } //transaction reference - order_id
        public string Description { get; set; }
        //public string webhook_url { get; set; }
    }

    public class PAEncryptedTransactionRequest
    {
        public string Payload { get; set; }
    }
}
