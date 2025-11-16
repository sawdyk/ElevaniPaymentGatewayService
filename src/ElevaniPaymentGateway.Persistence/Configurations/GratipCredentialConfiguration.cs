using ElevaniPaymentGateway.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElevaniPaymentGateway.Persistence.Configurations
{
    internal class GratipCredentialConfiguration : IEntityTypeConfiguration<GratipCredential>
    {
        public void Configure(EntityTypeBuilder<GratipCredential> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.APIKey).IsRequired().HasMaxLength(200);

            builder.Property(entity => entity.APISecret).IsRequired().HasMaxLength(200);


            builder.HasIndex(entity => new { entity.APIKey, entity.APISecret }).IsUnique();
        }
    }
}
