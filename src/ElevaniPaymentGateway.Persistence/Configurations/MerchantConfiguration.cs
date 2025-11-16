using ElevaniPaymentGateway.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElevaniPaymentGateway.Persistence.Configurations
{
    public class MerchantConfiguration : IEntityTypeConfiguration<Merchant>
    {
        public void Configure(EntityTypeBuilder<Merchant> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Name).IsRequired().HasMaxLength(200);

            builder.Property(entity => entity.Slug).IsRequired().HasMaxLength(10);

            builder.Property(entity => entity.Description).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.Address).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.Country).IsRequired(false).HasMaxLength(50);

            builder.Property(entity => entity.PhoneNumber).IsRequired(false).HasMaxLength(15);

            builder.Property(entity => entity.EmailAddress).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.PaymentGateway).IsRequired().HasConversion<string>();

            builder.Property(entity => entity.CreatedAt).IsRequired().HasDefaultValue(DateTime.Now);


            builder.HasIndex(entity => new { entity.Name }).IsUnique();
        }
    }
}
