using Asp.Versioning;
using ElevaniPaymentGateway.Core.Enums;
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
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Role by id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<RoleDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> IdAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _roleService.IdAsync(id);
            return Ok(response);
        }

        [HttpGet]
        [SwaggerOperation("All admin user and merchant roles")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<List<RoleDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> RolesAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _roleService.RolesAsync();
            return Ok(response);
        }

        [HttpGet("role-type/{roleType}")]
        [SwaggerOperation("Get roles by role type")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<List<RoleDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> RolesByRoleTypeAsync(RoleTypes roleType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _roleService.RolesByRoleTypeAsync(roleType);
            return Ok(response);
        }
    }
}
