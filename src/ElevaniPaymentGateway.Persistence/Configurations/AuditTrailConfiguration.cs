using ElevaniPaymentGateway.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElevaniPaymentGateway.Persistence.Configurations
{
    public class AuditTrailConfiguration : IEntityTypeConfiguration<AuditTrail>
    {
        public void Configure(EntityTypeBuilder<AuditTrail> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.UserId).IsRequired();

            builder.Property(entity => entity.IPAddress).IsRequired();

            builder.Property(entity => entity.Activity).IsRequired().HasConversion<string>();

            builder.Property(entity => entity.ActivityDetails).IsRequired(false);

            builder.Property(entity => entity.CreatedAt).IsRequired().HasDefaultValue(DateTime.Now);
        }
    }
}
