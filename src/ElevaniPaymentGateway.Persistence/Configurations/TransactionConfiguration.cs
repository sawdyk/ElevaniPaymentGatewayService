using ElevaniPaymentGateway.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElevaniPaymentGateway.Persistence.Configurations
{
    internal class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.MerchantId).IsRequired();

            builder.Property(entity => entity.Reference).IsRequired().HasMaxLength(200);

            builder.Property(entity => entity.Currency).IsRequired().HasMaxLength(10);

            builder.Property(entity => entity.Amount).IsRequired().HasDefaultValue(0).HasPrecision(15, 5);

            builder.Property(entity => entity.Narration).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.CountryCode).IsRequired(false).HasMaxLength(5);

            builder.Property(entity => entity.CustomerFirstName).IsRequired(false).HasMaxLength(100);

            builder.Property(entity => entity.CustomerLastName).IsRequired(false).HasMaxLength(100);

            builder.Property(entity => entity.CustomerEmail).IsRequired(false).HasMaxLength(100);

            builder.Property(entity => entity.CustomerPhoneNumber).IsRequired(false).HasMaxLength(15);

            builder.Property(entity => entity.PaymentGateway).HasConversion<string>();

            builder.Property(entity => entity.Status).HasConversion<string>();


            builder.HasIndex(entity => new { entity.MerchantId, entity.Reference }).IsUnique();
        }
    }
}
