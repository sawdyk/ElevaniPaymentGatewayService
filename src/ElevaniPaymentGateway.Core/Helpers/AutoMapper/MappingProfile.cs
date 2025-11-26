using AutoMapper;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Helpers.Pagination;
using ElevaniPaymentGateway.Core.Models.Dto;

namespace ElevaniPaymentGateway.Core.Helpers.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Role, RoleDto>();
            CreateMap<Transaction, TransactionDto>();
            CreateMap<PagedResult<Transaction>, PagedResult<TransactionDto>>();
        }
    }
}
