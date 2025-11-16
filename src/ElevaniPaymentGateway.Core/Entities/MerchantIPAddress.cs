using System.ComponentModel.DataAnnotations.Schema;

namespace ElevaniPaymentGateway.Core.Entities
{
    [Table(nameof(MerchantIPAddress))]
    public class MerchantIPAddress : BaseEntity
    {
        public string MerchantId { get; set; }
        public string IPAddress { get; set; }


        [ForeignKey("MerchantId")]
        public Merchant Merchant { get; set; }
    }
}
