using Asp.Versioning;
using ElevaniPaymentGateway.Infrastructure.Helpers;
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
        ValidationHelper _ValidationHelper;
        public TestController(ILogger<TestController> logger, IPayAgencyPaymentService payAgencyPaymentService,
            ValidationHelper validationHelper)
        {
            this.logger = logger;
            _payAgencyPaymentService = payAgencyPaymentService;
            _ValidationHelper = validationHelper;
        }

        [HttpPost("encrypt")]
        public async Task<IActionResult> EncryptData(string request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var encryptedData = PayAgencyEncryptionService.EncryptData(request, "2542b322a40ada01489c5491fe379512");

            logger.LogInformation($"encrypted data >>> {encryptedData}");

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

        //[HttpPost("validate")]
        //public async Task<IActionResult> validate(string val)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }

        //    var response =  _ValidationHelper.ValidateIPv4(val);
        //    return Ok(response);
        //}


        //[HttpPost("initiate")]
        //public async Task<IActionResult> Initiate(string request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }

        //    var response = await _payAgencyPaymentService.ServerToServerAsync(request);
        //    return Ok(response);
        //}
    }
}
