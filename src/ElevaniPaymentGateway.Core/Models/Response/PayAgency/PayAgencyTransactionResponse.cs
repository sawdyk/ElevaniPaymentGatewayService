namespace ElevaniPaymentGateway.Core.Models.Response.PayAgency
{
    public class PayAgencyTransactionResponse
    {
        public string? status { get; set; }
        public string? message { get; set; }
        public string? redirect_url { get; set; }
        public PayAgencyTransactionData? data { get; set; }
        public List<PayAgencyTransactionErrors>? errors { get; set; }
    }

    public class PayAgencyTransactionData
    {
        public decimal amount { get; set; }
        public string? currency { get; set; }
        public string? order_id { get; set; }
        public string? transaction_id { get; set; }
        public Customer? customer { get; set; }
        public Refund? refund { get; set; }
        public Chargeback? chargeback { get; set; }
    }

    public class Customer
    {
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? email { get; set; }
    }

    public class Refund
    {
        public bool status { get; set; }
        public object? refund_date { get; set; }
    }

    public class Chargeback
    {
        public bool status { get; set; }
        public object? chargeback_date { get; set; }
    }
    public class PayAgencyTransactionErrors
    {
        public string? field { get; set; }
        public string? message { get; set; }
    }
}
