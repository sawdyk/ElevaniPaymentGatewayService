using ElevaniPaymentGateway.Core.Enums;

namespace ElevaniPaymentGateway.Core.Models.Request
{
    public class MerchantRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public PaymentGateways PaymentGateway { get; set; }
    }
}
