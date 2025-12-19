using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Infrastructure.Autofac;
using System.Linq.Expressions;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Queries
{
    public interface IOTPQuery : IAutoDependencyServices
    {
        Task<IQueryable<OTP>> ListAllAsync(bool loadNavigationProps = false);
        Task<IQueryable<OTP>> ListAsync(Expression<Func<OTP, bool>> predicate, bool loadNavigationProps = false);
        Task<OTP> GetByAsync(Expression<Func<OTP, bool>> predicate, bool loadNavigationProps = false);
    }
}
