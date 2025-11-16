using Microsoft.AspNetCore.Identity;

namespace ElevaniPaymentGateway.Core.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }


        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
