using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElevaniPaymentGateway.Infrastructure.Extensions
{
    public static class PersistenceExtension
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, Role>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password = new PasswordOptions()
                {
                    RequiredLength = 8,
                };
                options.Lockout = new LockoutOptions()
                {
                    MaxFailedAccessAttempts = configuration.GetValue<int>("IdentityConfig:MaxFailedAccessAttempts"),
                    DefaultLockoutTimeSpan = TimeSpan.FromMinutes(configuration.GetValue<int>("IdentityConfig:DefaultLockoutTimeSpan")),
                    AllowedForNewUsers = configuration.GetValue<bool>("IdentityConfig:AllowedForNewUsers"),
                };
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("PGDatabaseCon"), b => b.MigrationsAssembly("ElevaniPaymentGateway.Persistence"))
                .EnableSensitiveDataLogging(false);
            });
        }
    }
}