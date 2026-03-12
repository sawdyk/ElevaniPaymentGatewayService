using ElevaniPaymentGateway.Core.Configs;
using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Helpers;
using ElevaniPaymentGateway.Core.Helpers.Pagination;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Interfaces.EfRepository;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Queries;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Helpers;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static ElevaniPaymentGateway.Core.Helpers.Pagination.QueryableExtensions;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly ILogger<MerchantCredentialService> _logger;
        private readonly IMerchantQuery _merchantQuery;
        private readonly IBaseRepository<Merchant> _merchantRepository;
        private readonly IHttpContextHelperService _httpContextHelperService;
        private UserContextDto _userContext;
        private readonly ISqlTransactionService _sqlTransactionService;
        private readonly IActivityLoggerService _activityLoggerService;
        private readonly IBaseRepository<MerchantCredential> _merchantCredentialRepository;
        private readonly AppSettingsConfig _appSettingsConfig;
        public MerchantService(ILogger<MerchantCredentialService> logger,
            IMerchantQuery merchantQuery, IBaseRepository<Merchant> merchantRepository, IHttpContextHelperService httpContextHelperService,
            ISqlTransactionService sqlTransactionService, IActivityLoggerService activityLoggerService, 
            IBaseRepository<MerchantCredential> merchantCredentialRepository, IOptions<AppSettingsConfig> appSettingsConfig)
        {
            _logger = logger;
            _merchantQuery = merchantQuery;
            _merchantRepository = merchantRepository;
            _httpContextHelperService = httpContextHelperService;
            _userContext = _httpContextHelperService.UserContext();
            _sqlTransactionService = sqlTransactionService;
            _activityLoggerService = activityLoggerService;
            _merchantCredentialRepository = merchantCredentialRepository;
            _appSettingsConfig = appSettingsConfig.Value;
        }

        public async Task<GenericResponse<Merchant>> CreateAsync(MerchantRequest request)
        {
            try
            {
                var merhantSlug = request.Name.Substring(0, 3);

                var merchant = new Merchant();
                merchant.Id = $"{merhantSlug.ToUpper()}{DateTime.Now.ToString("MMddyyyyhhmmss")}";
                merchant.Name = request.Name;
                merchant.Slug = merhantSlug.ToUpper();
                merchant.Description = request.Description;
                merchant.Address = request.Address;
                merchant.Country = request.Country;
                merchant.PhoneNumber = request.PhoneNumber;
                merchant.EmailAddress = request.EmailAddress;
                merchant.PaymentGateway = request.PaymentGateway;
                merchant.CreatedBy = _userContext.UserId.ToString();

                var transaction = await _sqlTransactionService.BeginTransactionAsync();

                _merchantRepository.Add(merchant);
                await _merchantRepository.SaveChangesAsync();

                var merchantCredential = new MerchantCredential();
                merchantCredential.MerchantId = merchant.Id;
                merchantCredential.APIKey = $"{Guid.NewGuid().ToString().Replace("-", "").ToLower()}";
                merchantCredential.APISecret = Guid.NewGuid().ToString().ToLower();
                merchantCredential.ExpiryDate = DateTime.Now.AddYears(_appSettingsConfig.MerchantCredentialExpiration);
                merchantCredential.CreatedBy = _userContext.UserId.ToString();

                _merchantCredentialRepository.Add(merchantCredential);
                await _merchantCredentialRepository.SaveChangesAsync();

                await _activityLoggerService.LogUserActivityAsync(ActivityTypes.Merchant, $"Created a new merchant : {request.Name}");

                await _sqlTransactionService.CommitAndDisposeTransactionAsync(transaction);

                return GenericResponse<Merchant>.Success(merchant);
            }
            catch (Exception ex)
             when (ex is NullReferenceException || ex is ArgumentNullException
            || ex.InnerException is SqlException)
            {
                throw ExceptionHandler.HandleExceptions(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to create a new merchant >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericPagedResponse<Merchant>> MerchantsAsync(PaginationParams paginationParams)
        {
            try
            {
                var merchantsQuery = await _merchantQuery.ListAllAsync();
                if (!string.IsNullOrWhiteSpace(paginationParams.SearchTerm))
                {
                    var searchConfig = new SearchConfig<Merchant>
                    {
                        SearchTerm = paginationParams.SearchTerm,
                        SearchProperties = new List<SearchProperty<Merchant>>
                        {
                            new(){ PropertyExpression = x => x.Id, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.Name, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.EmailAddress, SearchType = SearchType.Contains },
                        }
                    };

                    merchantsQuery = merchantsQuery.DynamicSearch(searchConfig);
                }

                var result = await merchantsQuery.ToPagedResultAsync(paginationParams);
                return GenericPagedResponse<Merchant>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to fecth all merchants - {ex.Message}" +
                    $"| stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse<Merchant>> IdAsync(string id)
        {
            try
            {
                var merchant = await _merchantQuery.GetByAsync(x => x.Id == id, true);
                if (merchant is null) throw new NotFoundException("Merchant does not exist");

                return GenericResponse<Merchant>.Success(merchant);
            }
            catch (Exception ex)
            when (ex is NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to get merchant by id - {ex.Message}" +
                    $"| stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse<Merchant>> SetStatusAsync(string id, bool isActive)
        {
            try
            {
                var merchant = await _merchantQuery.GetByAsync(x => x.Id == id, true);
                if (merchant is null) throw new NotFoundException("Merchant does not exist");

                merchant.IsActive = isActive;
                merchant.UpdatedBy = _userContext.UserId.ToString();
                merchant.UpdatedAt = DateTime.Now;

                _merchantRepository.Update(merchant);
                await _merchantRepository.SaveChangesAsync();

                return GenericResponse<Merchant>.Success(merchant);
            }
            catch (Exception ex)
            when (ex is NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to update merchant status - {ex.Message}" +
                    $"| stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
