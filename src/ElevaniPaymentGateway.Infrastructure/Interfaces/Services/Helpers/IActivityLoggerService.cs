using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Helpers
{
    public interface IActivityLoggerService : IAutoDependencyServices
    {
        Task LogUserActivityAsync(ActivityTypes activityType, string details);
        Task LogUserLoginActivityAsync(AuditTrail auditTrail);
    }
}
