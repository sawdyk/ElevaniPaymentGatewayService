using ElevaniPaymentGateway.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElevaniPaymentGateway.Persistence.Configurations
{
    internal class MerchantCredentialConfiguration : IEntityTypeConfiguration<MerchantCredential>
    {
        public void Configure(EntityTypeBuilder<MerchantCredential> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.MerchantId).IsRequired();

            builder.Property(entity => entity.APIKey).IsRequired().HasMaxLength(200);

            builder.Property(entity => entity.APISecret).IsRequired().HasMaxLength(200);

            builder.Property(entity => entity.WebhookURL).IsRequired(false).HasMaxLength(400);

            builder.Property(entity => entity.WebhookSecret).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.ExpiryDate).IsRequired();



            builder.HasIndex(entity => new { entity.MerchantId, entity.APIKey, entity.APISecret }).IsUnique();
        }
    }
}
