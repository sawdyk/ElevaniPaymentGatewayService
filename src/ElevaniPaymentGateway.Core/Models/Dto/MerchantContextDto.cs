namespace ElevaniPaymentGateway.Core.Models.Dto
{
    public class MerchantContextDto
    {
        public string? MerchantId { get; set; } 
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? PaymentGateway { get; set; }
    }
}
