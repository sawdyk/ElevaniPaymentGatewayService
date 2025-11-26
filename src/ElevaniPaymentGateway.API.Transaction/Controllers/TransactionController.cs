using Asp.Versioning;
using ElevaniPaymentGateway.Core.Helpers.Pagination;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Request.TransactionService;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Core.Models.Response.TransactionService;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElevaniPaymentGateway.PaymentAPI.Controllers
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

        [HttpPost("initiate")]
        [SwaggerOperation("Initiate transaction")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<TransactionResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> InitiateTransactionViaPaymentGatewayAsync(TransactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _transactionService.InitiateTransactionViaPaymentGatewayAsync(request);
            return Ok(response);
        }

        [HttpGet("status/{reference}")]
        [SwaggerOperation("Get transaction status by reference")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<TransactionDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> StatusAsync(string reference)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _transactionService.StatusAsync(reference);
            return Ok(response);
        }

        [HttpGet("transactions")]
        [SwaggerOperation("Get all merchant transactions")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<TransactionDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> MerchantIdAsync([FromQuery] PaginationParams paginationParams)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _transactionService.MerchantAsync(paginationParams);
            return Ok(response);
        }
    }
}
