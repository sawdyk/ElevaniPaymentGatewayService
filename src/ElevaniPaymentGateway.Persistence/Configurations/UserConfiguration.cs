using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElevaniPaymentGateway.Persistence.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Email).IsRequired().HasMaxLength(100);

            builder.Property(entity => entity.FirstName).IsRequired().HasMaxLength(100);

            builder.Property(entity => entity.LastName).IsRequired().HasMaxLength(100);

            builder.Property(entity => entity.PasswordHash).IsRequired();

            builder.Property(entity => entity.EmailConfirmed).HasDefaultValue(false);

            builder.Property(entity => entity.DateOfBirth).HasDefaultValue(DateTime.Now);

            builder.Property(entity => entity.Status).HasDefaultValue(UserStatus.Active).HasConversion<string>();



            builder.HasIndex(entity => new { entity.Email }).IsUnique();

            // Each User can have many entries in the UserRole join table
            builder.HasMany(e => e.UserRoles).WithOne(e => e.User).HasForeignKey(ur => ur.UserId).IsRequired();
        }
    }
}
