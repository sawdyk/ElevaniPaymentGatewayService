using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services.Helpers
{
    public class PaymentHttpContextHelperService : IPaymentHttpContextHelperService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PaymentHttpContextHelperService> _logger;
        public PaymentHttpContextHelperService(IHttpContextAccessor httpContextAccessor,
           ILogger<PaymentHttpContextHelperService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public MerchantContextDto MerchantContext()
        {
            try
            {
                var merchantId = _httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == PaymentClaimTypesHelpers.MerchantId).FirstOrDefault()?.Value ?? "";
                var name = _httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == PaymentClaimTypesHelpers.Name).FirstOrDefault()?.Value ?? "";
                var slug = _httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == PaymentClaimTypesHelpers.Slug).FirstOrDefault()?.Value ?? "";
                var paymentGateway = _httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == PaymentClaimTypesHelpers.PaymentGateway).FirstOrDefault()?.Value ?? "";

                var userResult = new MerchantContextDto
                {
                    MerchantId = merchantId,
                    Name = name,
                    Slug = slug,
                    PaymentGateway = paymentGateway,
                };

                return userResult;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to user data from httpcontext claims - {ex.Message}" +
                    $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
