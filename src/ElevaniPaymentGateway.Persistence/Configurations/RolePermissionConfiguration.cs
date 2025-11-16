using ElevaniPaymentGateway.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElevaniPaymentGateway.Persistence.Configurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermissions>
    {
        public void Configure(EntityTypeBuilder<RolePermissions> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.PermissionId).IsRequired().HasMaxLength(100);

            builder.Property(entity => entity.RoleId).IsRequired().HasMaxLength(100);


            builder.HasIndex(entity => new { entity.PermissionId, entity.RoleId }).IsUnique();
        }
    }
}
