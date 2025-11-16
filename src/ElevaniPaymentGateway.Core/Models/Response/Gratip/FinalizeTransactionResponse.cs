namespace ElevaniPaymentGateway.Core.Models.Response.Gratip
{
    public class FinalizeTransactionResponse
    {
        public bool success { get; set; }
        public string? message { get; set; }
        public FinalizeTransactionResponseData? data { get; set; }
    }

    public class FinalizeTransactionResponseData
    {
        public int collection_id { get; set; }
        public string? transaction_reference { get; set; }
        public string? external_reference { get; set; }
        public string? status { get; set; }
        public string? amount { get; set; }
        public string? currency { get; set; }
        public string? client_name { get; set; }
        public DateTime? completed_at { get; set; }
    }
}
