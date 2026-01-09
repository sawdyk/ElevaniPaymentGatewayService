using ElevaniPaymentGateway.Core.Configs;
using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Interfaces.EfRepository;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Queries;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services
{
    public class MerchantCredentialService : IMerchantCredentialService
    {
        private readonly ILogger<MerchantCredentialService> _logger;
        private readonly IJWTUtilityService _jwtUtilityService;
        private readonly IMerchantCredentialQuery _merchantCredentialQuery;
        private readonly IMerchantQuery _merchantQuery;
        private readonly AppSettingsConfig _appSettingsConfig;
        private readonly IBaseRepository<MerchantCredential> _merchantCredentialRepository;
        public MerchantCredentialService(ILogger<MerchantCredentialService> logger, IJWTUtilityService jwtUtilityService,
            IMerchantCredentialQuery merchantCredentialQuery, IMerchantQuery merchantQuery, IOptions<AppSettingsConfig> appSettingsConfig,
            IBaseRepository<MerchantCredential> merchantCredentialRepository)
        {
            _logger = logger;
            _jwtUtilityService = jwtUtilityService;
            _merchantCredentialQuery = merchantCredentialQuery;
            _merchantQuery = merchantQuery;
            _appSettingsConfig = appSettingsConfig.Value;
            _merchantCredentialRepository = merchantCredentialRepository;
        }

        public async Task<GenericResponse<JwtResponse>> GenerateAuthenticationTokenAsync(MerchantAuthTokenRequest request)
        {
            try
            {
               var merchantCredential = await _merchantCredentialQuery.GetByAsync(x => x.APIKey ==  request.APIKey && x.APISecret == request.APISecret, true);
                if(merchantCredential is null) throw new GenericException("Invalid credentials");
                if (DateTime.Now > merchantCredential.ExpiryDate) throw new GenericException("Expired credentials. Kindly generate new credentials");

                var jwtResponse = await _jwtUtilityService.GenerateMerchantAuthenticationAccessToken(merchantCredential.Merchant);
                return GenericResponse<JwtResponse>.Success(jwtResponse);
            }
            catch (Exception ex)
            when (ex is GenericException || ex is NotFoundException)
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

        public async Task<GenericResponse<MerchantCredential>> GenerateCredentialsAsync(MerchantCredentialRequest request)
        {
            try
            {
                var merchant = await _merchantQuery.GetByAsync(x => x.Id == request.MerchantId);
                if (merchant is null) throw new NotFoundException("Merchant does not exist");

                var merchantCredential = await _merchantCredentialQuery.GetByAsync(x => x.MerchantId == request.MerchantId);
                if (merchantCredential is null) throw new NotFoundException("Merchant credential does not exist");

                merchantCredential.MerchantId = request.MerchantId;
                merchantCredential.APIKey = $"sk_{Guid.NewGuid().ToString().Replace("-", "").ToLower()}";
                merchantCredential.APISecret = Guid.NewGuid().ToString().ToLower();
                merchantCredential.ExpiryDate = DateTime.Now.AddMonths(_appSettingsConfig.MerchantCredentialExpiration);

                _merchantCredentialRepository.Update(merchantCredential);
                await _merchantCredentialRepository.SaveChangesAsync();

                return GenericResponse<MerchantCredential>.Success(merchantCredential);
            }
            catch (Exception ex)
            when (ex is GenericException || ex is NotFoundException)
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
    }
}
