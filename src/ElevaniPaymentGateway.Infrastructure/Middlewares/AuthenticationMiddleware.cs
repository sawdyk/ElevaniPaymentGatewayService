using ElevaniPaymentGateway.Core.Configs;
using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Text.Json;

namespace ElevaniPaymentGateway.Infrastructure.Middlewares
{
    public class AuthenticationMiddleware : IMiddleware
    {
        private readonly ILogger<AuthenticationMiddleware> _logger;
        private AppSettingsConfig _appSettingsConfig;
        private string _apiKeyHeader = string.Empty;
        private string _apiSecretHeader = string.Empty;
        private readonly IMerchantCredentialQuery _merchantCredentialQuery;
        public AuthenticationMiddleware(ILogger<AuthenticationMiddleware> logger, IOptions<AppSettingsConfig> appSettingsConfig,
            IMerchantCredentialQuery merchantCredentialQuery)
        {
            _logger = logger;
            _appSettingsConfig = appSettingsConfig.Value;
            _apiKeyHeader = _appSettingsConfig.APIKeyHeader;
            _apiSecretHeader = _appSettingsConfig.APISecretHeader;
            _merchantCredentialQuery = merchantCredentialQuery;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                StringValues apiKey;
                StringValues apiSecret;
                context.Request.Headers.TryGetValue(_apiKeyHeader, out apiKey);
                context.Request.Headers.TryGetValue(_apiSecretHeader, out apiSecret);

                var merchantApikey = apiKey.FirstOrDefault();
                var merchantApiSecret = apiSecret.FirstOrDefault();

                ProblemDetails problemDetails = new ProblemDetails();

                if (string.IsNullOrEmpty(merchantApikey) && string.IsNullOrEmpty(merchantApiSecret))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }

                var merchantCredential = await _merchantCredentialQuery.GetByAsync(x => x.APIKey == merchantApikey && x.APISecret == merchantApiSecret);

                if (merchantCredential is null)
                {
                    problemDetails.Status = (int)HttpStatusCode.Unauthorized;
                    problemDetails.Type = "UnAuthorized";
                    problemDetails.Title = "UnAuthorized";
                    problemDetails.Detail = "Invalid Credentials";

                    string json = JsonSerializer.Serialize(problemDetails);
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync(json);

                    _logger.LogError($"Invalid merchant credentials");

                    return;
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    _logger.LogInformation($"valid merchant credentials");
                    await next(context);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to authenticate merchant - {ex.Message}" +
                     $"| stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
