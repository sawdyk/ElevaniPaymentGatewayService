using ElevaniPaymentGateway.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElevaniPaymentGateway.Persistence.Configurations
{
    public class PermissionsConfiguration : IEntityTypeConfiguration<Permissions>
    {
        public void Configure(EntityTypeBuilder<Permissions> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Name).IsRequired().HasMaxLength(100);

            builder.Property(entity => entity.Description).IsRequired(false).HasMaxLength(200);


            builder.HasIndex(entity => new { entity.Name }).IsUnique();
        }
    }
}
