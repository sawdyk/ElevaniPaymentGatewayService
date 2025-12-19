using ElevaniPaymentGateway.Infrastructure.Middlewares;
using Microsoft.Extensions.DependencyInjection;

namespace ElevaniPaymentGateway.Infrastructure.Extensions
{
    public static class MiddlewareExtension
    {
        public static void AddAdminMiddleware(this IServiceCollection services)
        {
            services.AddScoped<GlobalExceptionMiddleware>();
            services.AddScoped<ResponseHeaderMiddleware>(); 
        }

        public static void AddPaymentServiceMiddleware(this IServiceCollection services)
        {
            services.AddScoped<GlobalExceptionMiddleware>();
            services.AddScoped<ResponseHeaderMiddleware>();
            services.AddScoped<IPAddressWhitelistMiddleware>();
        }
    }
}
