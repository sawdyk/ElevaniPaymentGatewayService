using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElevaniPaymentGateway.WorkerService.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation("Ping")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(string))]
        public async Task<IActionResult> PingAsync()
        {
            return Ok("pong");
        }
    }
}
