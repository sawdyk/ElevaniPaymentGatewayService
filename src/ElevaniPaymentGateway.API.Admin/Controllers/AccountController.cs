using Asp.Versioning;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElevaniPaymentGateway.API.Admin.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("verify-OTP")]
        [SwaggerOperation("Verify OTP for account verification and forgot password")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<UserDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> VerifyOTPAsync(ValidateOTPRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _accountService.VerifyOTPAsync(request);
            return Ok(response);
        }

        [Authorize]
        [HttpPut("change-password")]
        [SwaggerOperation("Change password")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<UserDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _accountService.ChangePasswordAsync(request);
            return Ok(response);
        }


        [HttpPost("forgot-password")]
        [SwaggerOperation("Forgot password")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<UserDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> ForgotPasswordAsync(string emailAddress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _accountService.ForgotPasswordAsync(emailAddress);
            return Ok(response);
        }


        [HttpPost("reset-password")]
        [SwaggerOperation("Reset password")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<UserDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _accountService.ResetPasswordAsync(resetPasswordRequest);
            return Ok(response);
        }

        [HttpPost("OTP")]
        [SwaggerOperation("Generate new OTP for forgot password and account verification")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<OTP>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> GenerateOTPAsync(GenerateOTPRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _accountService.GenerateOTPAsync(request);
            return Ok(response);
        }
    }
}
