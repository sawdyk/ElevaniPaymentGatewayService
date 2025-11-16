using ElevaniPaymentGateway.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace ElevaniPaymentGateway.Infrastructure.Middlewares
{
    public class GlobalExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            when (ex is BaseException baseException)
            {
                _logger.LogError($"{baseException.StatusCode}  >>>  {baseException.Message}");
                context.Response.StatusCode = (int)baseException.StatusCode;

                ProblemDetails problemDetails = new()
                {
                    Status = (int)baseException.StatusCode,
                    Type = baseException.StatusCode.ToString() + " error",
                    Title = baseException.StatusCode.ToString() + " error",
                    Detail = baseException.Message,
                };

                string json = JsonSerializer.Serialize(problemDetails);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"An error occured >> {ex.Message}--->> StackTrace>> {ex.StackTrace} --->> Inner Exception>>{ex.InnerException}--->> Source>> {ex.Source}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ProblemDetails problemDetails = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Internal server error",
                    Title = "Internal server error",
                    Detail = "An error occurred",
                };

                string json = JsonSerializer.Serialize(problemDetails);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
        }
    }
}
