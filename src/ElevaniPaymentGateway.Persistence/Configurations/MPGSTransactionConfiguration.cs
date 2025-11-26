using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevaniPaymentGateway.Persistence.Configurations
{
    internal class MPGSTransactionConfiguration : IEntityTypeConfiguration<MPGSTransaction>
    {
        public void Configure(EntityTypeBuilder<MPGSTransaction> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.TransactionId).IsRequired();

            builder.Property(entity => entity.BillingCountry).IsRequired(false).HasMaxLength(100);

            builder.Property(entity => entity.BillingAddress).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.BillingCity).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.BillingState).IsRequired(false).HasMaxLength(50);

            builder.Property(entity => entity.BillingZipCode).IsRequired(false).HasMaxLength(50);

            builder.Property(entity => entity.RedirectUrl).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.CardName).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.CardType).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.CardFirst6).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.CardLast4).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.ExpiryMonth).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.ExpiryYear).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.Token).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.AuthorizationUrl).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.AuthorizationMethod).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.IsVerified).IsRequired().HasDefaultValue(false);

            builder.Property(entity => entity.DateVerified).IsRequired(false).HasDefaultValue(null);

            builder.Property(entity => entity.LastRetryDateTime).IsRequired(false).HasDefaultValue(null);

            builder.Property(entity => entity.Status).IsRequired().HasDefaultValue(TransactionStatus.Pending).HasConversion<string>();
        }
    }
}
