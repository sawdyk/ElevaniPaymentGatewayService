using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services.Helpers
{
    public class HttpContextHelperService : IHttpContextHelperService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<HttpContextHelperService> _logger;
        public HttpContextHelperService(IHttpContextAccessor httpContextAccessor,
           ILogger<HttpContextHelperService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public UserContextDto UserContext()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == ClaimTypesHelpers.UserId).FirstOrDefault()?.Value ?? "";
                var userName = _httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == ClaimTypesHelpers.UserName).FirstOrDefault()?.Value ?? "";
                var firstName = _httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == ClaimTypesHelpers.FirstName).FirstOrDefault()?.Value ?? "";
                var lastName = _httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == ClaimTypesHelpers.LastName).FirstOrDefault()?.Value ?? "";
                var emailAddress = _httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == ClaimTypesHelpers.EmailAddress).FirstOrDefault()?.Value ?? "";
                var phoneNumber = _httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == ClaimTypesHelpers.PhoneNumber).FirstOrDefault()?.Value ?? "";
                var role = _httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == ClaimTypesHelpers.Role).FirstOrDefault()?.Value ?? "";
                var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

                var userResult = new UserContextDto
                {
                    UserId = Guid.Parse(userId),
                    FirstName = firstName,
                    LastName = lastName,
                    EmailAddress = emailAddress,
                    PhoneNumber = phoneNumber,
                    Role = role,
                    IPAddress = ipAddress,
                };

                return userResult;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to user data from httpcontext claims - {ex.Message}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
