using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ElevaniPaymentGateway.Infrastructure.Middlewares
{
    public class ResponseHeaderMiddleware : IMiddleware
    {
        private readonly ILogger<ResponseHeaderMiddleware> _logger;
        public ResponseHeaderMiddleware(ILogger<ResponseHeaderMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                context.Response.Headers["Cache-Cotrol"] = "no-cache, no-store, must-revalidate";
                context.Response.Headers["Pragma"] = "no-cache";
                context.Response.Headers["Expires"] = "0";
                context.Response.Headers.Remove("X-Powered-By");
                context.Response.Headers.Remove("Server");
                context.Response.Headers["X-Frame-Oprions"] = "DENY";
                context.Response.Headers["X-Xss-Protection"] = "1; mode=block";
                context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";

                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                    $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
            }
        }
    }
}
