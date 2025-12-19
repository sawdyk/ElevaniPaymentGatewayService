using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;


namespace ElevaniPaymentGateway.Infrastructure.Extensions
{
    public static class AppLoggerExtension
    {
        public static void AddAppLogging(this IHostBuilder hostBuilder)
        {
            //Logging
            hostBuilder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            }).UseSerilog(((ctx, lc) => lc
            .ReadFrom.Configuration(ctx.Configuration)
            .Enrich.FromLogContext()));
        }
    }
}
