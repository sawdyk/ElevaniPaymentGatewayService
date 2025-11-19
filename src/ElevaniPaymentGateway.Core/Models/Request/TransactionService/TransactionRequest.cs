namespace ElevaniPaymentGateway.Core.Models.Request.TransactionService
{
    public class TransactionRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Reference { get; set; }
        public string CountryCode { get; set; }
        public string? Description { get; set; }
        //public string? CustomerFirstName { get; set; }
        //public string? CustomerLastName { get; set; }
        //public string? CustomerEmail { get; set; }
        //public string? CustomerPhoneNumber { get; set; }
    }
}
