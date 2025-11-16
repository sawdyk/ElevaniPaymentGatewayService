using System.ComponentModel.DataAnnotations.Schema;

namespace ElevaniPaymentGateway.Core.Entities
{
    [Table(nameof(MerchantCredential))]
    public class MerchantCredential : BaseEntity
    {
        public string MerchantId { get; set; }
        public string APIKey { get; set; }
        public string APISecret { get; set; }
        public string? WebhookURL { get; set; }
        public string? WebhookSecret { get; set; }
        public DateTime ExpiryDate { get; set; } //force merchants to generate new API keys every 30 days


        [ForeignKey("MerchantId")]
        public Merchant Merchant { get; set; }
    }
}
