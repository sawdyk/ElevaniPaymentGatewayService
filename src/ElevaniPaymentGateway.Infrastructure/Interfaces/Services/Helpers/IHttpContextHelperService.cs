using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Helpers
{
    public interface IHttpContextHelperService : IAutoDependencyServices
    {
        UserContextDto UserContext();
    }
}
