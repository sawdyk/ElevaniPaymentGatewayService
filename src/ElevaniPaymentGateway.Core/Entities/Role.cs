using ElevaniPaymentGateway.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace ElevaniPaymentGateway.Core.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public RoleTypes? RoleType { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<RolePermissions> RolePermissions { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
