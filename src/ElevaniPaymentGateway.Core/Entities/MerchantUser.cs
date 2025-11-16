using System.ComponentModel.DataAnnotations.Schema;

namespace ElevaniPaymentGateway.Core.Entities
{
    [Table(nameof(MerchantUser))]
    public class MerchantUser : BaseEntity
    {
        public string MerchantId { get; set; }
        public Guid UserId { get; set; }


        [ForeignKey("MerchantId")]
        public Merchant Merchant { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
