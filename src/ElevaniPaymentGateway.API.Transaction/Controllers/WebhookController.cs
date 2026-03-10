using Asp.Versioning;
using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.PaymentGateway.Gratip;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

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

                if(string.IsNullOrEmpty(payload))
                    throw new GenericException("Webhook request body is empty");

                //request headers
                var headers = HttpContext.Request.Headers; 
                StringValues headerAuthorizedSignature;
                var headerAuthSignature = headers.TryGetValue("x-gratip-signature", out headerAuthorizedSignature);
                var signature = headerAuthorizedSignature.FirstOrDefault();

                if (string.IsNullOrEmpty(signature))
                    throw new GenericException("No x-gratip-signature found in the request header");

                _logger.LogInformation($"Webhook signature: {signature}");
                _logger.LogInformation($"Attempting to verify webhook signature......");

                if (!await _gratipWebhookService.VerifyWebhookSignature(payload, signature))
                {
                    _logger.LogError($"Invalid webhook signature");
                    throw new GenericException("$Invalid webhook signature");
                }

                _logger.LogInformation($"Valid webhook signature");
                await _gratipWebhookService.HandleWebhookNotification(payload);

                return new HttpResponseMessage(HttpStatusCode.OK); //returns 200 
            }
            catch (Exception ex)
            when (ex is GenericException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to handle Gratip webhook notification >> " +
                    $"{ex.Message} | stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
