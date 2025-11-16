using ElevaniPaymentGateway.Infrastructure.Implementations.EfRepository;
using ElevaniPaymentGateway.Infrastructure.Interfaces.EfRepository;
using Microsoft.Extensions.DependencyInjection;

namespace ElevaniPaymentGateway.Infrastructure.Extensions
{
    public static class RepositoryExtension
    {
        public static void AddRepositoryExtension(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        }
    }
}
