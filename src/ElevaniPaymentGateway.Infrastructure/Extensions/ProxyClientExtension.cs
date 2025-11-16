using ElevaniPaymentGateway.Infrastructure.Implementations.ProxyClients.Gratip;
using ElevaniPaymentGateway.Infrastructure.Interfaces.ProxyClients.Gratip;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElevaniPaymentGateway.Infrastructure.Extensions
{
    public static class ProxyClientExtension
    {
        public static void RegisterServicesHttpProxy(this IServiceCollection services)
        {
            services.AddScoped<IGratipCredentialService, GratipCredentialService>();
            services.AddScoped<IGratipCollectionService, GratipCollectionService>();
        }

        public static IServiceCollection RegisterServicesHttpProxyClient(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            //var serviceProvider = host.Services.CreateScope().ServiceProvider;
            //var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            //var gratipCredentials =  dbContext.GratipSecurityKey.FirstOrDefault();

            serviceCollection.AddHttpClient<IGratipServiceProxyClient, GratipServiceProxyClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["GratipConfig:BaseUrl"]);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("x-api-key", configuration["GratipConfig:APIKey"]);
                client.DefaultRequestHeaders.Add("x-api-secret", configuration["GratipConfig:APISecret"]);
            });

            return serviceCollection;
        }
    }
}
