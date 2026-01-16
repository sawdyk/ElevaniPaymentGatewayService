using Asp.Versioning;
using ElevaniPaymentGateway.Infrastructure.Implementations.Services.PaymentGateway.PayAgency;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.PaymentGateway.PayAgency;
using Microsoft.AspNetCore.Mvc;

namespace ElevaniPaymentGateway.API.Transaction.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> logger;
        IPayAgencyPaymentService _payAgencyPaymentService;
        public TestController(ILogger<TestController> logger, IPayAgencyPaymentService payAgencyPaymentService)
        {
            this.logger = logger;
            _payAgencyPaymentService = payAgencyPaymentService;
        }

        [HttpPost("encrypt")]
        public async Task<IActionResult> EncryptData(string request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var encryptedData = PayAgencyEncryptionService.EncryptData(request, "2542b322a40ada01489c5491fe379512");

            logger.LogInformation($"encypted data >>> {encryptedData}");

            return Ok(encryptedData);
        }

        [HttpPost("decrypt")]
        public async Task<IActionResult> DecryptData(string request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var encryptedData = PayAgencyEncryptionService.DecryptData(request, "2542b322a40ada01489c5491fe379512");

            logger.LogInformation($"decrypted data >>> {encryptedData}");

            return Ok(encryptedData);
        }

        [HttpPost("initiate")]
        public async Task<IActionResult> Initiate(string request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _payAgencyPaymentService.InitiateTransactionAsync("", request);
            return Ok(response);
        }
    }
}
