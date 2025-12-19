using ElevaniPaymentGateway.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElevaniPaymentGateway.Persistence.Configurations
{
    public class OTPConfiguration : IEntityTypeConfiguration<OTP>
    {
        public void Configure(EntityTypeBuilder<OTP> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.UserId).IsRequired();

            builder.Property(entity => entity.OTPType).IsRequired().HasConversion<string>();

            builder.Property(entity => entity.OTPValue).IsRequired().HasMaxLength(6);

            builder.Property(entity => entity.TokenValue).IsRequired().HasMaxLength(500);

            builder.Property(entity => entity.IsUsed).IsRequired().HasDefaultValue(false);

            builder.Property(entity => entity.DateGenerated).IsRequired().HasDefaultValue(DateTime.Now);

            builder.Property(entity => entity.ExpiryDateTime).IsRequired();

            builder.Property(entity => entity.DateUsed).HasDefaultValue(null);

            builder.HasIndex(entity => new { entity.UserId, entity.OTPType }).IsUnique();
        }
    }
}
