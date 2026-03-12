using ElevaniPaymentGateway.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElevaniPaymentGateway.Core.Entities
{
    [Table(nameof(Merchant))]
    public class Merchant
    {
        [Key]
        public string Id { get; set; } //unique alphanumeric - M11102025454321
        public string Name { get; set; }
        public string Slug { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public PaymentGateways PaymentGateway { get; set; }
        public bool IsActive { get; set; } = true;
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }


        public virtual MerchantCredential MerchantCredential { get; set; }
        public virtual ICollection<MerchantIPAddress> MerchantIPAddresses { get; set; }
    }
}
