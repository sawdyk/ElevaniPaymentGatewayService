using Autofac;
using Autofac.Extensions.DependencyInjection;
using ElevaniPaymentGateway.Infrastructure.Autofac;
using Microsoft.Extensions.Hosting;

namespace ElevaniPaymentGateway.Infrastructure.Extensions
{
    public static class AutoFacExtension
    {
        public static void AddAutoFacExtension(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterModule(new AutofacContainerServicesModule());
                builder.RegisterModule(new AutofacRepositoryModule());
            });
        }
    }
}
