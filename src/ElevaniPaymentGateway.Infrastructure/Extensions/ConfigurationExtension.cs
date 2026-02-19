using Asp.Versioning;
using ElevaniPaymentGateway.Core.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElevaniPaymentGateway.Infrastructure.Extensions
{
    public static class ConfigurationExtension
    {
        public static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));
            services.Configure<AppSettingsConfig>(configuration.GetSection("AppSettingsConfig"));
            services.Configure<IdentityConfig>(configuration.GetSection("IdentityConfig"));
            services.Configure<GratipConfig>(configuration.GetSection("GratipConfig"));
            services.Configure<EmailConfig>(configuration.GetSection("EmailConfig"));
            services.Configure<JobIDConfig>(configuration.GetSection("JobIDConfig"));
            services.Configure<EncryptorConfig>(configuration.GetSection("EncryptorConfig"));
            services.Configure<BackgroundJobConfig>(configuration.GetSection("BackgroundJobConfig"));
            services.Configure<PayAgencyConfig>(configuration.GetSection("PayAgencyConfig"));

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                //options.ApiVersionReader = ApiVersionReader.Combine(
                //    new UrlSegmentApiVersionReader(),
                //    new HeaderApiVersionReader("X-Api-Version"));
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
        }
    }
}
