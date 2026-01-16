using ElevaniPaymentGateway.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElevaniPaymentGateway.Persistence.Configurations
{
    internal class PayAgencyTransactionConfiguration : IEntityTypeConfiguration<PayAgencyTransaction>
    {
        public void Configure(EntityTypeBuilder<PayAgencyTransaction> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.TransactionId).IsRequired();

            builder.Property(entity => entity.Reference).IsRequired().HasMaxLength(200);

            builder.Property(entity => entity.FirstName).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.LastName).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.Email).IsRequired(false).HasMaxLength(500);

            builder.Property(entity => entity.Address).IsRequired(false);

            builder.Property(entity => entity.Country).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.City).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.State).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.Zip).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.IPAddress).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.PhoneNumber).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.Amount).HasDefaultValue(0).HasPrecision(15, 5);

            builder.Property(entity => entity.Description).IsRequired(false).HasMaxLength(500);

            builder.Property(entity => entity.Currency).IsRequired(false).HasMaxLength(10);

            builder.Property(entity => entity.CardNumber).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.CardExpiryMonth).IsRequired(false).HasMaxLength(10);

            builder.Property(entity => entity.CardExpiryYear).IsRequired(false).HasMaxLength(10);

            builder.Property(entity => entity.CardCVV).IsRequired(false).HasMaxLength(10);

            builder.Property(entity => entity.RedirectUrl).IsRequired(false);

            builder.Property(entity => entity.OTPRedirectUrl).IsRequired(false);

            builder.Property(entity => entity.WebHookUrl).IsRequired(false);

            builder.Property(entity => entity.IsVerified).IsRequired().HasDefaultValue(null);

            builder.Property(entity => entity.DateVerified).IsRequired(false).HasDefaultValue(null);

            builder.Property(entity => entity.LastRetryDateTime).IsRequired(false).HasDefaultValue(null);

            builder.Property(entity => entity.Status).IsRequired().HasConversion<string>();
        }
    }
}
