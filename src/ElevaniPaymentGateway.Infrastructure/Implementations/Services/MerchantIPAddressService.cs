using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Helpers;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Interfaces.EfRepository;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Queries;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Helpers;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services
{
    public class MerchantIPAddressService : IMerchantIPAddressService
    {
        private readonly ILogger<MerchantIPAddressService> _logger;
        private readonly IBaseRepository<MerchantIPAddress> _merchantIPAddressRepository;
        private readonly IHttpContextHelperService _httpContextHelperService;
        private UserContextDto _userContext;
        private readonly ISqlTransactionService _sqlTransactionService;
        private readonly IActivityLoggerService _activityLoggerService;
        private readonly IMerchantIPAddressQuery _merchantIPAddressQuery;
        public MerchantIPAddressService(ILogger<MerchantIPAddressService> logger,
            IBaseRepository<MerchantIPAddress> merchantIPAddressRepository, IHttpContextHelperService httpContextHelperService,
            ISqlTransactionService sqlTransactionService, IActivityLoggerService activityLoggerService, IMerchantIPAddressQuery merchantIPAddressQuery)
        {
            _logger = logger;
            _merchantIPAddressRepository = merchantIPAddressRepository;
            _httpContextHelperService = httpContextHelperService;
            _userContext = _httpContextHelperService.UserContext();
            _sqlTransactionService = sqlTransactionService;
            _activityLoggerService = activityLoggerService;
            _merchantIPAddressQuery = merchantIPAddressQuery;
        }

        public async Task<GenericResponse<MerchantIPAddress>> CreateAsync(MerchantIPAddressRequest request)
        {
            try
            {
                var merchantIPAdresses = await(await _merchantIPAddressQuery.ListAsync(x => x.MerchantId == request.MerchantId)).ToListAsync();
                if(merchantIPAdresses.Count() == 5) throw new GenericException("Merchant can only whitelist 5 IP addresses");

                var merchantIPAddress = new MerchantIPAddress();
                merchantIPAddress.MerchantId = request.MerchantId;
                merchantIPAddress.IPAddress = request.IPAddress;
                merchantIPAddress.CreatedBy = _userContext.UserId.ToString();

                var transaction = await _sqlTransactionService.BeginTransactionAsync();

                _merchantIPAddressRepository.Add(merchantIPAddress);
                await _merchantIPAddressRepository.SaveChangesAsync();

                await _activityLoggerService.LogUserActivityAsync(ActivityTypes.MerchantIPAddress, 
                    $"Created a new merchant IP address for whitelisting : {request.IPAddress}");

                await _sqlTransactionService.CommitAndDisposeTransactionAsync(transaction);

                return GenericResponse<MerchantIPAddress>.Success(merchantIPAddress);
            }
            catch (Exception ex)
             when (ex is NullReferenceException || ex is ArgumentNullException
            || ex.InnerException is SqlException || ex is GenericException)
            {
                if (ex is GenericException)
                    throw;
                throw ExceptionHandler.HandleExceptions(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to create a new merchant IP address >> " +
                    $"{ex.Message} | stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse<MerchantIPAddress>> IdAsync(Guid id)
        {
            try
            {
                var merchantIPaddress = await _merchantIPAddressQuery.GetByAsync(x => x.Id == id, true);
                if (merchantIPaddress is null) throw new NotFoundException("Merchant IP address does not exist");

                return GenericResponse<MerchantIPAddress>.Success(merchantIPaddress);
            }
            catch (Exception ex)
            when (ex is NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to get merchant IP address by id - {ex.Message}" +
                    $"| stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse> DeleteAsync(Guid id)
        {
            try
            {
                var merchantIPAddress = await _merchantIPAddressQuery.GetByAsync(x => x.Id == id);
                if (merchantIPAddress == null) throw new NotFoundException(RespMsgConstants.NotFound);

                var transaction = await _sqlTransactionService.BeginTransactionAsync();

                _merchantIPAddressRepository.Delete(merchantIPAddress);
                await _merchantIPAddressRepository.SaveChangesAsync();

                await _activityLoggerService.LogUserActivityAsync(ActivityTypes.MerchantIPAddress,
                    $"Deleted a merchant IP address");

                await _sqlTransactionService.CommitAndDisposeTransactionAsync(transaction);

                return GenericResponse.Success("Merchant IP address deleted successfully");
            }
            catch (Exception ex)
            when (ex is NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to delete a merchant IP address - {ex.Message}" +
                    $"| stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse<List<MerchantIPAddress>>> MerchantIdAsync(string merchantId)
        {
            try
            {
                var merchantIPaddresses = await (await _merchantIPAddressQuery.ListAsync(x => x.MerchantId == merchantId))
                    .ToListAsync();
              
                return GenericResponse<List<MerchantIPAddress>>.Success(merchantIPaddresses);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to fecth all merchant IP address - {ex.Message}" +
                    $"| stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse<MerchantIPAddress>> UpdateAsync(Guid id, MerchantIPAddressRequest request)
        {
            try
            {
                var merchantIPAddress = await _merchantIPAddressQuery.GetByAsync(x => x.Id == id);
                if (merchantIPAddress is null) throw new GenericException(RespMsgConstants.NotFound);

                merchantIPAddress.MerchantId = request.MerchantId;
                merchantIPAddress.IPAddress = request.IPAddress;
                merchantIPAddress.UpdatedBy = _userContext.UserId.ToString();
                merchantIPAddress.UpdatedAt = DateTime.UtcNow;

                var transaction = await _sqlTransactionService.BeginTransactionAsync();

                _merchantIPAddressRepository.Update(merchantIPAddress);
                await _merchantIPAddressRepository.SaveChangesAsync();

                await _activityLoggerService.LogUserActivityAsync(ActivityTypes.MerchantIPAddress,
                    $"Updated merchant IP address for whitelisting : {request.IPAddress}");

                await _sqlTransactionService.CommitAndDisposeTransactionAsync(transaction);

                return GenericResponse<MerchantIPAddress>.Success(merchantIPAddress);
            }
            catch (Exception ex)
             when (ex is NullReferenceException || ex is ArgumentNullException
            || ex.InnerException is SqlException || ex is GenericException)
            {
                if (ex is GenericException)
                    throw;
                throw ExceptionHandler.HandleExceptions(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to update merchant IP address >> " +
                    $"{ex.Message} | stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
