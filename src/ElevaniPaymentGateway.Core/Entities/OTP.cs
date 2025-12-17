using ElevaniPaymentGateway.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElevaniPaymentGateway.Core.Entities
{
    [Table(nameof(OTP))]
    public class OTP : BaseEntity
    {
        public Guid UserId { get; set; }
        public OTPTypes OTPType { get; set; }
        public string OTPValue { get; set; }
        public string TokenValue { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime DateGenerated { get; set; } = DateTime.Now;
        public DateTime ExpiryDateTime { get; set; }
        public DateTime? DateUsed { get; set; }


        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
