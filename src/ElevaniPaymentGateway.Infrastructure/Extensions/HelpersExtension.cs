using AutoMapper;
using ElevaniPaymentGateway.Core.Helpers;
using ElevaniPaymentGateway.Core.Helpers.AutoMapper;
using ElevaniPaymentGateway.Infrastructure.Helpers;
using Microsoft.Extensions.DependencyInjection;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace ElevaniPaymentGateway.Infrastructure.Extensions
{
    public static class HelpersExtension
    {
        public static void AddHelpersExtension(this IServiceCollection services)
        {
            services.AddScoped<AESAlgoHelper>();
            services.AddScoped<ValidationHelper>();

            //Autoper
            var perConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper per = perConfig.CreateMapper();
            services.AddSingleton(per);

            services.AddSingleton(typeof(IConverter), new
            SynchronizedConverter(new PdfTools()));
        }
    }
}
