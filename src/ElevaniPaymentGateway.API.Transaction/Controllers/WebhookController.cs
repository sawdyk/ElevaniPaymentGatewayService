using Asp.Versioning;
using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Core.Models.Response.TransactionService;
using ElevaniPaymentGateway.Infrastructure.Implementations.Services.PaymentGateway.Gratip;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.PaymentGateway.Gratip;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Text.Json;

namespace ElevaniPaymentGateway.API.Transaction.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly IGratipWebhookService _gratipWebhookService;
        private readonly ILogger<WebhookController> _logger;
        public WebhookController(IGratipWebhookService gratipWebhookService, ILogger<WebhookController> logger)
        {
            _gratipWebhookService = gratipWebhookService;
            _logger = logger;
        }

        [HttpPost("gratip")]
        [SwaggerOperation("Gratip webhook")]
        public async Task<HttpResponseMessage> HandleWebhook()
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback
                    = ((sender, certificate, chain, sslPolicyErrors) => true);

                //the incoming event from gratip
                var payload = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                _logger.LogInformation($"Webhook payload: {payload}");

                //request headers
                var headers = HttpContext.Request.Headers; 
                StringValues headerAuthorizedSignature;
                var headerAuthSignature = headers.TryGetValue("x-gratip-signature", out headerAuthorizedSignature);
                var signature = headerAuthorizedSignature.FirstOrDefault();

                _logger.LogInformation($"Webhook signature: {signature}");
                _logger.LogInformation($"Attempting to verify webhook signature......");

                if (!await _gratipWebhookService.VerifyWebhookSignature(payload, signature))
                {
                    _logger.LogError($"Invalid webhook signature");
                    return new HttpResponseMessage(HttpStatusCode.BadRequest); //returns 400 
                }

                _logger.LogInformation($"Valid webhook signature");
                await _gratipWebhookService.HandleWebhookNotification(payload);

                return new HttpResponseMessage(HttpStatusCode.OK); //returns 200 
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to handle Gratip webhook notification >> " +
                    $"{ex.Message} | stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError); //returns 500
            }
        }
    }
}
