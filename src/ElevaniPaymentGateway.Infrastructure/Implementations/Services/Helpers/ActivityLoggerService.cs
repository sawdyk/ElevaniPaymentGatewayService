using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Infrastructure.Interfaces.EfRepository;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Helpers;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services.Helpers
{
    public class ActivityLoggerService : IActivityLoggerService
    {
        private readonly IHttpContextHelperService _httpContextHelperService;
        private readonly IBaseRepository<AuditTrail> _auditTrailRepository;
        public ActivityLoggerService(IHttpContextHelperService httpContextHelperService,
            IBaseRepository<AuditTrail> auditTrailRepository)
        {
            _auditTrailRepository = auditTrailRepository;
            _httpContextHelperService = httpContextHelperService;

        }

        public async Task LogUserActivityAsync(ActivityTypes activityType, string details)
        {
            var user = _httpContextHelperService.UserContext();

            var auditTrail = new AuditTrail
            {
                UserId = user.UserId,
                IPAddress = user.IPAddress,
                Activity = activityType,
                ActivityDetails = details,
            };

            _auditTrailRepository.Add(auditTrail);
            await _auditTrailRepository.SaveChangesAsync();
        }

        public async Task LogUserLoginActivityAsync(AuditTrail auditTrail)
        {
            var newAuditTrail = new AuditTrail
            {
                UserId = auditTrail.UserId,
                IPAddress = auditTrail.IPAddress,
                Activity = auditTrail.Activity,
                ActivityDetails = auditTrail.ActivityDetails,
            };

            _auditTrailRepository.Add(auditTrail);
            await _auditTrailRepository.SaveChangesAsync();
        }
    }
}
