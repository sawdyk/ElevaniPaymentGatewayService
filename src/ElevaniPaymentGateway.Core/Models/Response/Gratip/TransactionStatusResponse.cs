namespace ElevaniPaymentGateway.Core.Models.Response.Gratip
{
    public class TransactionStatusResponse
    {
        public bool success { get; set; }
        public string? message { get; set; }
        public TransactionStatusResponseData? data { get; set; }
    }

    public class TransactionStatusResponseData
    {
        public int collection_id { get; set; }
        public string? transaction_reference { get; set; }
        public string? external_reference { get; set; }
        public string? amount { get; set; }
        public string? currency { get; set; }
        public string? status { get; set; }
        public string? client_name { get; set; }
        public Fees? fees { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
