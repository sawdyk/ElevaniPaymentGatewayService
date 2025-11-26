using System.ComponentModel.DataAnnotations.Schema;

namespace ElevaniPaymentGateway.Core.Entities
{
    [Table(nameof(Permissions))]
    public class Permissions : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
       
        public virtual ICollection<RolePermissions> RolePermissions { get; set; }
    }
}
