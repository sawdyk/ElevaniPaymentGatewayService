using AutoMapper;
using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Queries;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services
{
    public class RoleService : IRoleService
    {
        private readonly ILogger<RoleService> _logger;
        private readonly IMapper _mapper;
        private readonly IRoleQuery _roleQuery;
        public RoleService(ILogger<RoleService> logger,
            IMapper mapper, IRoleQuery roleQuery)
        {
            _logger = logger;
            _mapper = mapper;
            _roleQuery = roleQuery;
        }

        public async Task<GenericResponse<RoleDto>> IdAsync(Guid id)
        {
            try
            {
                var role = (await _roleQuery.GetByAsync(x => x.Id == id, false));
                if (role is null) throw new NotFoundException("role does not exist");

                var roleDto = _mapper.Map<RoleDto>(role);

                return GenericResponse<RoleDto>.Success(roleDto);
            }
            catch (Exception ex)
            when (ex is NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse<List<RoleDto>>> RolesAsync()
        {
            try
            {
                var roles = await (await _roleQuery.ListAllAsync(false)).ToListAsync();

                var roleDto = _mapper.Map<List<RoleDto>>(roles);
                return GenericResponse<List<RoleDto>>.Success(roleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse<List<RoleDto>>> RolesByRoleTypeAsync(RoleTypes roleType)
        {
            try
            {
                var roles = await (await _roleQuery.ListAsync(x => x.RoleType == roleType))
                    .ToListAsync();

                var roleDto = _mapper.Map<List<RoleDto>>(roles);
                return GenericResponse<List<RoleDto>>.Success(roleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

    }
}
