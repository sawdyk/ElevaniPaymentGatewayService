namespace ElevaniPaymentGateway.Core.Models.Response.Gratip
{
    public class InitiateTransctionResponse
    {
        public bool success { get; set; }
        public string? message { get; set; }
        public InitiateTransactionResponseData? data { get; set; }
    }

    public class InitiateTransactionResponseData
    {
        public int collection_id { get; set; }
        public string? request_reference { get; set; }
        public string? transaction_reference { get; set; }
        public string? payment_url { get; set; }
        public string? external_reference { get; set; }
        public decimal amount { get; set; }
        public string? currency { get; set; }
        public string? status { get; set; }
        public Fees? fees { get; set; }
        public DateTime? created_at { get; set; }
    }

    public class Fees
    {
        public decimal system_fee { get; set; }
        public decimal system_fee_percentage { get; set; }
        public decimal client_fee { get; set; }
        public decimal client_fee_percentage { get; set; }
        public decimal total_fees { get; set; }
        public decimal net_amount { get; set; }
    }
}
