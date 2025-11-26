using ElevaniPaymentGateway.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElevaniPaymentGateway.Persistence.Configurations
{
    internal class MerchantUserConfiguration : IEntityTypeConfiguration<MerchantUser>
    {
        public void Configure(EntityTypeBuilder<MerchantUser> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.MerchantId).IsRequired();

            builder.Property(entity => entity.UserId).IsRequired();


            builder.HasIndex(entity => new { entity.MerchantId, entity.UserId }).IsUnique();
        }
    }
}
