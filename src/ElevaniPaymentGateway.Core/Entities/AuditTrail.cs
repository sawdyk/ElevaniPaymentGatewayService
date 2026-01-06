using ElevaniPaymentGateway.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElevaniPaymentGateway.Core.Entities
{
    [Table(nameof(AuditTrail))]
    public class AuditTrail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string IPAddress { get; set; }
        public ActivityTypes Activity { get; set; }
        public string? ActivityDetails { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
