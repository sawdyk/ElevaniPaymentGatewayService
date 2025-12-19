using Asp.Versioning;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Helpers.Pagination;
using ElevaniPaymentGateway.Core.Models.Request;
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
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("transaction")]
        [SwaggerOperation("Transactions report")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<ReportResponse<Transaction>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> TransactionsReportAsync(TransactionReportRequest request, 
            [FromQuery] PaginationParams paginationParams)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _reportService.TransactionsReportAsync(request, paginationParams);
            return Ok(response);
        }
    }
}
