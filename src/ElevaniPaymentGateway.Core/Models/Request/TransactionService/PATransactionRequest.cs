using System.ComponentModel.DataAnnotations;

namespace ElevaniPaymentGateway.Core.Models.Request.TransactionService
{
    public class PATransactionRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Zip { get; set; }
        [Required]
        public string IPAddress { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string CardExpiryMonth { get; set; }
        [Required]
        public string CardExpiryYear { get; set; }
        [Required]
        public string CardCVV { get; set; }
        [Required]
        public string RedirectUrl { get; set; }
        [Required]
        public string Reference { get; set; } //transaction reference - order_id
        [Required]
        public string Description { get; set; }
        //public string webhook_url { get; set; }
    }
}
