namespace ElevaniPaymentGateway.Core.Models.Request.PayAgency
{
    public class PayAgencyTransactionRequest
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string ip_address { get; set; }
        public string phone_number { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public string card_number { get; set; }
        public string card_expiry_month { get; set; }
        public string card_expiry_year { get; set; }
        public string card_cvv { get; set; }
        public string redirect_url { get; set; }
        //public string webhook_url { get; set; }
        public string order_id { get; set; }
    }

    public class PayAgencyEncryptedRequest
    {
        public string payload { get; set; }
    }
}
