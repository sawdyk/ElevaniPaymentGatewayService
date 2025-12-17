using Asp.Versioning;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElevaniPaymentGateway.MerchantAPI.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        [SwaggerOperation("Admin and Merchant users login endpoint")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<UserLoginResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> LoginAsync(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _authenticationService.LoginAsync(request);
            return Ok(response);
        }
        
        [HttpPost("refresh-token")]
        [SwaggerOperation("Generate a refresh token")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<LoginResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _authenticationService.RefreshToken(request);
            return Ok(response);
        }
    }
}
