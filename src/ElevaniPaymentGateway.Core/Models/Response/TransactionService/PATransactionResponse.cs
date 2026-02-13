using ElevaniPaymentGateway.Core.Models.Response.PayAgency;

namespace ElevaniPaymentGateway.Core.Models.Response.TransactionService
{
    public class PATransactionResponse
    {
        public string MerchantID { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string Reference { get; set; }
        public string TransactionReference { get; set; }
        public string? RedirectUrl { get; set; } //for 3DS card transaction
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public PACustomerDetails? Customer { get; set; }
        //public List<PayAgencyTransactionErrors>? Errors { get; set; }
    }

    public class PACustomerDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
