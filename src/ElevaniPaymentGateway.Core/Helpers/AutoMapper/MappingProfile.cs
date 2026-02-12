using AutoMapper;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Helpers.Pagination;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Request.PayAgency;
using ElevaniPaymentGateway.Core.Models.Request.TransactionService;

namespace ElevaniPaymentGateway.Core.Helpers.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Role, RoleDto>();
            CreateMap<UserRole, UserRoleDto>();
            CreateMap<PagedResult<UserRole>, PagedResult<UserRoleDto>>();
            CreateMap<PagedResult<User>, PagedResult<UserDto>>();
            CreateMap<Transaction, TransactionDto>();
            CreateMap<PagedResult<Transaction>, PagedResult<TransactionDto>>();
            CreateMap<GratipTransaction, GratipTransactionDto>();
            CreateMap<PagedResult<GratipTransaction>, PagedResult<GratipTransactionDto>>();
            CreateMap<PayAgencyTransaction, PayAgencyTransactionDto>();
            CreateMap<PagedResult<PayAgencyTransaction>, PagedResult<PayAgencyTransactionDto>>();
            CreateMap<Transaction, MerchantTransactionDto>();
            CreateMap<PagedResult<Transaction>, PagedResult<MerchantTransactionDto>>();

            CreateMap<PATransactionRequest, PayAgencyTransactionRequest>()
                .ForMember(dest => dest.first_name, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.last_name, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.city, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.state, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.zip, opt => opt.MapFrom(src => src.Zip))
                .ForMember(dest => dest.ip_address, opt => opt.MapFrom(src => src.IPAddress))
                .ForMember(dest => dest.phone_number, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.card_number, opt => opt.MapFrom(src => src.CardNumber))
                .ForMember(dest => dest.card_expiry_month, opt => opt.MapFrom(src => src.CardExpiryMonth))
                .ForMember(dest => dest.card_expiry_year, opt => opt.MapFrom(src => src.CardExpiryYear))
                .ForMember(dest => dest.card_cvv, opt => opt.MapFrom(src => src.CardCVV))
                .ForMember(dest => dest.redirect_url, opt => opt.MapFrom(src => src.RedirectUrl))
                .ForMember(dest => dest.order_id, opt => opt.MapFrom(src => src.Reference));
        }
    }
}
