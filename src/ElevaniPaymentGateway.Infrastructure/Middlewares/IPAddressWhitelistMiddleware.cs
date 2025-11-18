using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ElevaniPaymentGateway.Infrastructure.Middlewares
{
    public class IPAddressWhitelistMiddleware : IMiddleware
    {
        private readonly ILogger<IPAddressWhitelistMiddleware> _logger;
        private readonly IMerchantIPAddressQuery _merchantIPAddressQuery;
        public IPAddressWhitelistMiddleware(ILogger<IPAddressWhitelistMiddleware> logger,
            IMerchantIPAddressQuery merchantIPAddressQuery)
        {
            _logger = logger;
            _merchantIPAddressQuery = merchantIPAddressQuery;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            { 
                var _whitelist =  await (await _merchantIPAddressQuery.ListAllAsync())
                    .Select(x => x.IPAddress)
                    .ToListAsync();

                var remoteIp = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
                if (remoteIp.Contains("::ffff:"))
                    remoteIp = remoteIp.Replace("::ffff:", "");

                if (!_whitelist.Contains(remoteIp))
                {
                    ProblemDetails problemDetails = new ProblemDetails();
                    problemDetails.Status = StatusCodes.Status403Forbidden;
                    problemDetails.Type = "Forbidden";
                    problemDetails.Title = "Forbidden";
                    problemDetails.Detail = $"Unauthorized access to call this service";

                    string json = JsonSerializer.Serialize(problemDetails);
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync(json);

                    _logger.LogError($"IP aadress {remoteIp} is not a whitelisted IP address to call the service");
                    return;
                }

                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered at IP address whitelist middleware while trying to authenticate merchant IP address - {ex.Message}" +
                     $"| stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
