namespace ElevaniPaymentGateway.Core.Models.Response.Gratip
{
    public class GratipWebhookResponse
    {
        public string? @event { get; set; }
        public DateTime? timestamp { get; set; }
        public GratipWebhookData? data { get; set; }
    }

    public class GratipWebhookData
    {
        public string? external_reference { get; set; }
        public string? transaction_reference { get; set; }
        public float? amount { get; set; }
        public string? currency { get; set; }
        public string? status { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? completed_at { get; set; }
        public string? masked_pan { get; set; }
        public string? billing_name { get; set; }
        public string? card_brand { get; set; }
        public string? payment_method { get; set; }
        //public Fees fees { get; set; }
        //public Currency_Conversion currency_conversion { get; set; }
        //public Payment_Details payment_details { get; set; }
    }

    public class Currency_Conversion
    {
        public string? from { get; set; }
        public string? to { get; set; }
        public float? rate { get; set; }
        public float? original_amount { get; set; }
        public float? charged_amount { get; set; }
        public DateTime? conversion_timestamp { get; set; }
    }

    public class Payment_Details
    {
    }
}
