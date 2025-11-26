namespace ElevaniPaymentGateway.Core.Models.Request.Gratip
{
    public class InitiateTransactionRequest
    {
        public decimal amount { get; set; } // Required: Amount to collect (minimum 0.01)
        public string currency { get; set; } // Optional: ISO currency code (USD, GBP, EUR) - defaults to USD
        public string method { get; set; } // Required: Payment method (Google Pay, Apple Pay, Card Pay)
        public string countryCode { get; set; }  // Required: 2-letter country code
        public string external_reference { get; set; } // Optional: Your internal reference (auto-generated if not provided)
        public string? description { get; set; } // Optional: Payment description (max 500 chars)
        //public string? merchant_id { get; set; } // Optional: Merchant identifier for tracking (max 100 chars)
        //public Customer_Info? customer_info { get; set; } // Optional: Customer information
    }
    public class Customer_Info
    {
        public string? firstName { get; set; }  // Optional: Customer first name (max 100 chars)
        public string? lastName { get; set; } // Optional: Customer last name (max 100 chars)
        public string? email { get; set; } // Optional: Customer email
    }

}
