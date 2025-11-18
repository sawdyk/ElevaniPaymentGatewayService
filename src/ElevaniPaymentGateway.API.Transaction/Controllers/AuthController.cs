using Asp.Versioning;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElevaniPaymentGateway.PaymentAPI.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMerchantCredentialService _merchantCredentialService;
        public AuthController(IMerchantCredentialService merchantCredentialService)
        {
            _merchantCredentialService = merchantCredentialService;
        }

        [HttpPost("token")]
        [SwaggerOperation("Generate a new token for authentication. Token expires after 24hrs")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<JwtResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> GenerateAuthenticationTokenAsync(MerchantAuthTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _merchantCredentialService.GenerateAuthenticationTokenAsync(request);
            return Ok(response);
        }
    }
}
