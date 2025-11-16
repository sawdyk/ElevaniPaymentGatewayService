using System.ComponentModel.DataAnnotations.Schema;

namespace ElevaniPaymentGateway.Core.Entities
{
    [Table(nameof(RolePermissions))]
    public class RolePermissions : BaseEntity
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }


        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [ForeignKey("PermissionId")]
        public Permissions Permissions { get; set; }
    }
}
