using Asp.Versioning;
using ElevaniPaymentGateway.Core.Entities;
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
    public class MerchantIPAddressController : ControllerBase
    {
        private readonly IMerchantIPAddressService _merchantIPAddressService;
        public MerchantIPAddressController(IMerchantIPAddressService merchantIPAddressService)
        {
            _merchantIPAddressService = merchantIPAddressService;
        }

        [HttpPost]
        [SwaggerOperation("Create a new merchant IP address")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<MerchantIPAddress>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> CreateAsync(MerchantIPAddressRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _merchantIPAddressService.CreateAsync(request);
            return Ok(response);
        }

        [HttpGet("merchant/{merchantId}")]
        [SwaggerOperation("Get merchant IP addresses")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<List<MerchantIPAddress>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> MerchantIdAsync(string merchantId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _merchantIPAddressService.MerchantIdAsync(merchantId);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Get merchant IP address by id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<Merchant>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> IdAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _merchantIPAddressService.IdAsync(id);
            return Ok(response);
        }

        [HttpPut("{id}")]
        [SwaggerOperation("Update merchant IP address")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<MerchantIPAddress>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateAsync(Guid id, MerchantIPAddressRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _merchantIPAddressService.UpdateAsync(id, request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Delete merchant IP address")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _merchantIPAddressService.DeleteAsync(id);
            return Ok(response);
        }
    }
}
