using Asp.Versioning;
using ElevaniPaymentGateway.Core.Helpers.Pagination;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElevaniPaymentGateway.API.Admin.Controllers
{
    [Authorize]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("merchant/{merchantId}")]
        [SwaggerOperation("Get all merchant transactions")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<TransactionDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> MerchantIdAsync(string merchantId, [FromQuery] PaginationParams paginationParams)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _transactionService.MerchantIdAsync(merchantId, paginationParams);
            return Ok(response);
        }
    }
}
