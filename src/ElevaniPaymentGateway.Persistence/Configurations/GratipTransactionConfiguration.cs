using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElevaniPaymentGateway.Persistence.Configurations
{
    internal class GratipTransactionConfiguration : IEntityTypeConfiguration<GratipTransaction>
    {
        public void Configure(EntityTypeBuilder<GratipTransaction> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.TransactionId).IsRequired();

            builder.Property(entity => entity.ExternalReference).IsRequired().HasMaxLength(200);

            builder.Property(entity => entity.Currency).IsRequired().HasMaxLength(5);

            builder.Property(entity => entity.Method).IsRequired().HasMaxLength(200);

            builder.Property(entity => entity.Amount).HasDefaultValue(0).HasPrecision(15, 5);

            builder.Property(entity => entity.CountryCode).IsRequired().HasMaxLength(15);

            builder.Property(entity => entity.MerchantID).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.Description).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.CustomerEmail).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.CustomerFirstName).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.CustomerLastName).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.CollectionId).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.RequestReference).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.TransactionReference).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.PaymentURL).IsRequired(false);

            builder.Property(entity => entity.SystemFee).HasDefaultValue(0).HasPrecision(15, 5);

            builder.Property(entity => entity.SystemFeePercentage).HasDefaultValue(0).HasPrecision(15, 5);

            builder.Property(entity => entity.ClientFee).HasDefaultValue(0).HasPrecision(15, 5);

            builder.Property(entity => entity.ClientFeePercentage).HasDefaultValue(0).HasPrecision(15, 5);

            builder.Property(entity => entity.TotalFees).HasDefaultValue(0).HasPrecision(15, 5);

            builder.Property(entity => entity.NetAmount).HasDefaultValue(0).HasPrecision(15, 5);

            builder.Property(entity => entity.IsVerified).IsRequired().HasDefaultValue(false);

            builder.Property(entity => entity.DateVerified).IsRequired(false).HasDefaultValue(null);

            builder.Property(entity => entity.LastRetryDateTime).IsRequired(false).HasDefaultValue(null);

            builder.Property(entity => entity.Status).IsRequired().HasDefaultValue(TransactionStatus.Pending).HasConversion<string>();
        }
    }
}
