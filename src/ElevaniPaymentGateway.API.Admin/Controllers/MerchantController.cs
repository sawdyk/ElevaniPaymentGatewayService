using Asp.Versioning;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Helpers.Pagination;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElevaniPaymentGateway.MerchantAPI.Controllers
{
    [Authorize]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class MerchantController : ControllerBase
    {
        private readonly IMerchantService _merchantService;
        private readonly IMerchantCredentialService _merchantCredentialService;
        public MerchantController(IMerchantService merchantService, IMerchantCredentialService merchantCredentialService)
        {
            _merchantService = merchantService;
            _merchantCredentialService = merchantCredentialService;
        }

        [HttpPost]
        [SwaggerOperation("Create a new merchant")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<Merchant>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> CreateAsync(MerchantRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _merchantService.CreateAsync(request);
            return Ok(response);
        }

        [HttpGet]
        [SwaggerOperation("Get all merchants")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericPagedResponse<Merchant>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> MerchantsAsync([FromQuery] PaginationParams paginationParams)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _merchantService.MerchantsAsync(paginationParams);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Get merchant by id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<Merchant>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> IdAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _merchantService.IdAsync(id);
            return Ok(response);
        }

        [HttpPost("credential")]
        [SwaggerOperation("Generate a new credential for merchants")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<MerchantCredential>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> GenerateCredentialsAsync(MerchantCredentialRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _merchantCredentialService.GenerateCredentialsAsync(request);
            return Ok(response);
        }
    }
}
