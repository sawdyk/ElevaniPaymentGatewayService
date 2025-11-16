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

        [HttpPost("merchant")]
        [SwaggerOperation("Merchant login")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<LoginResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> MerchantLoginAsync(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _authenticationService.MerchantLoginAsync(request);
            return Ok(response);
        }

        [HttpPost("admin")]
        [SwaggerOperation("Admin user login")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<LoginResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> AdminLoginAsync(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _authenticationService.AdminLoginAsync(request);
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
