using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services
{
    public interface IOTPService : IAutoDependencyServices
    {
        Task<OTP> GenerateOTPAsync(User user, OTPTypes oTPType);
    }
}
