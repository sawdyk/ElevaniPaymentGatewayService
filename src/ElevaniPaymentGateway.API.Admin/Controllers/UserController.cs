using Asp.Versioning;
using ElevaniPaymentGateway.Core.Helpers.Pagination;
using ElevaniPaymentGateway.Core.Models.Dto;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("admin-user")]
        [SwaggerOperation("Create a new admin user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<UserDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> CreateAdminUserAsync(AdminUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _userService.CreateAdminUserAsync(request);
            return Ok(response);
        }

        [HttpPost("merchant-user")]
        [SwaggerOperation("Create a new merchant user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<UserDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> CreateMerchantUserAsync(MerchantUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _userService.CreateMerchantUserAsync(request);
            return Ok(response);
        }

        [HttpPut("admin-user")]
        [SwaggerOperation("Update an admin user details")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<UserDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateAdminUserAsync(UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _userService.UpdateAdminUserAsync(request);
            return Ok(response);
        }

        [HttpPut("merchant-user")]
        [SwaggerOperation("Update a merchant user detauls")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<UserDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateMerchantUserAsync(UpdateMerchantUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _userService.UpdateMerchantUserAsync(request);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Get user by id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<UserRoleDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> UserByIdAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _userService.UserByIdAsync(id);
            return Ok(response);
        }

        [HttpGet("merchant-users")]
        [SwaggerOperation("Get all merchant users")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericPagedResponse<UserRoleDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> MerchantUsersAsync([FromQuery] PaginationParams paginationParams)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _userService.MerchantUsersAsync(paginationParams);
            return Ok(response);
        }

        [HttpGet("merchant-users/{merchantId}")]
        [SwaggerOperation("Get all merchant users by merchant id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericPagedResponse<UserDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> MerchantUsersByMerchantIdAsync(string merchantId, [FromQuery] PaginationParams paginationParams)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _userService.MerchantUsersByMerchantIdAsync(merchantId, paginationParams);
            return Ok(response);
        }

        [HttpGet("admin-users")]
        [SwaggerOperation("Get all admin users")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericPagedResponse<UserRoleDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> AdminUsersAsync([FromQuery] PaginationParams paginationParams)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _userService.AdminUsersAsync(paginationParams);
            return Ok(response);
        }

        [HttpPut("set-status")]
        [SwaggerOperation("Set user status - active/inactive")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse<UserDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> SetStatusAsync(UserStatusRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _userService.SetStatusAsync(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Delete an admin or merchant user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(GenericResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _userService.DeleteAsync(id);
            return Ok(response);
        }
    }
}
