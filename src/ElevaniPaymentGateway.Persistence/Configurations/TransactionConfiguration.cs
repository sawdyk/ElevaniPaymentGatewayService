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

            builder.Property(entity => entity.Narration).IsRequired(false).HasMaxLength(500);

            builder.Property(entity => entity.CountryCode).IsRequired(false).HasMaxLength(10);

            builder.Property(entity => entity.FirstName).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.LastName).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.Email).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.PhoneNumber).IsRequired(false).HasMaxLength(50);

            builder.Property(entity => entity.City).IsRequired(false).HasMaxLength(100);

            builder.Property(entity => entity.State).IsRequired(false).HasMaxLength(100);

            builder.Property(entity => entity.Zip).IsRequired(false).HasMaxLength(100);

            builder.Property(entity => entity.IPAddress).IsRequired(false).HasMaxLength(100);

            builder.Property(entity => entity.CardNumber).IsRequired(false).HasMaxLength(100);

            builder.Property(entity => entity.CardExpiryMonth).IsRequired(false).HasMaxLength(50);

            builder.Property(entity => entity.CardExpiryYear).IsRequired(false).HasMaxLength(50);

            builder.Property(entity => entity.CardCVV).IsRequired(false).HasMaxLength(20);

            builder.Property(entity => entity.RedirectUrl).IsRequired(false);

            builder.Property(entity => entity.WebHookUrl).IsRequired(false);

            builder.Property(entity => entity.PaymentGateway).HasConversion<string>();

            builder.Property(entity => entity.Status).HasConversion<string>();


            builder.HasIndex(entity => new { entity.MerchantId, entity.Reference }).IsUnique();
        }
    }
}
