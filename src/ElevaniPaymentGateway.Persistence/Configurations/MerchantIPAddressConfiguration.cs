using ElevaniPaymentGateway.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElevaniPaymentGateway.Persistence.Configurations
{
    internal class MerchantIPAddressConfiguration : IEntityTypeConfiguration<MerchantIPAddress>
    {
        public void Configure(EntityTypeBuilder<MerchantIPAddress> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.MerchantId).IsRequired();

            builder.Property(entity => entity.IPAddress).IsRequired();


            builder.HasIndex(entity => new { entity.MerchantId, entity.IPAddress }).IsUnique();
        }
    }
}
