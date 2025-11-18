using AutoMapper;
using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Helpers.Pagination;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Request.TransactionService;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Core.Models.Response.TransactionService;
using ElevaniPaymentGateway.Infrastructure.Helpers;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Queries;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Helpers;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.PaymentGateway.Gratip;
using Microsoft.Extensions.Logging;
using static ElevaniPaymentGateway.Core.Helpers.Pagination.QueryableExtensions;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ILogger<TransactionService> _logger;
        private readonly ITransactionQuery _transactionQuery;
        private readonly IMapper _mapper;
        private readonly IGratipPaymentService _gratipPaymentService;
        private MerchantContextDto _merchantContext;
        private readonly IPaymentHttpContextHelperService _paymentHttpContextHelperService;
        private readonly ValidationHelper _validationHelper;
        public TransactionService(ILogger<TransactionService> logger, ITransactionQuery transactionQuery, IMapper mapper, 
            IGratipPaymentService gratipPaymentService, IPaymentHttpContextHelperService paymentHttpContextHelperService, ValidationHelper validationHelper)
        {
            _logger = logger;
            _transactionQuery = transactionQuery;
            _mapper = mapper;
            _gratipPaymentService = gratipPaymentService;
            _paymentHttpContextHelperService = paymentHttpContextHelperService;
            _merchantContext = _paymentHttpContextHelperService.MerchantContext();
            _validationHelper = validationHelper;
        }

        public async Task<GenericResponse<TransactionResponse>> InitiateTransactionViaPaymentGatewayAsync(TransactionRequest request)
        {
            try
            {
                _validationHelper.ValidateRequest(request);
                TransactionResponse initiatePaymentResponse = new TransactionResponse();

                var merhantSlug = request.Reference.Substring(0, 3);
                if(!_merchantContext.Slug.Equals(merhantSlug))
                    throw new GenericException("Invalid transaction reference format");

                var existingReference = await _transactionQuery.GetByAsync(x => x.Reference == request.Reference);
                if (existingReference is not null)
                    throw new GenericException("Duplicate transaction reference");
                //string reference = RandomGeneratorHelper.GenerateTransactionReference(_merchantContext.Slug, request.Currency, request.Amount.ToString());

                switch (_merchantContext.PaymentGateway)
                {
                    case nameof(PaymentGateways.GRATIP):
                        initiatePaymentResponse = await _gratipPaymentService.InitiateTransactionAsync(_merchantContext.MerchantId, request);
                        break;

                    default:
                        break;
                }

                return GenericResponse<TransactionResponse>.Success(initiatePaymentResponse);
            }
            catch (Exception ex)
            when (ex is NotFoundException || ex is GenericException
            || ex is DataValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to initiate payment via payment gateway >> " +
                    $"{ex.Message} | stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse<TransactionDto>> StatusAsync(string reference)
        {
            try
            {
                var transaction = await _transactionQuery.GetByAsync(x => x.Reference == reference && x.MerchantId == _merchantContext.MerchantId);
                if (transaction is null) throw new NotFoundException("Transaction does not exist");

                var transactionDto = _mapper.Map<TransactionDto>(transaction);
                return GenericResponse<TransactionDto>.Success(transactionDto);
            }
            catch (Exception ex)
            when (ex is NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to get transaction by reference - {ex.Message}" +
                    $"| stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericPagedResponse<TransactionDto>> MerchantIdAsync(string merchantId, PaginationParams paginationParams)
        {
            try
            {
                var transactionsQuery = await _transactionQuery.ListAsync(x => x.MerchantId == merchantId);
                if (!string.IsNullOrWhiteSpace(paginationParams.SearchTerm))
                {
                    var searchConfig = new SearchConfig<Transaction>
                    {
                        SearchTerm = paginationParams.SearchTerm,
                        SearchProperties = new List<SearchProperty<Transaction>>
                        {
                            new(){ PropertyExpression = x => x.Reference, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.Currency, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.Amount, SearchType = SearchType.Contains },
                        }
                    };

                    transactionsQuery = transactionsQuery.DynamicSearch(searchConfig);
                }

                var result = await transactionsQuery.ToPagedResultAsync(paginationParams);
                var transactionsDto = _mapper.Map<PagedResult<TransactionDto>>(result);

                return GenericPagedResponse<TransactionDto>.Success(transactionsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to fecth all merchant transactions - {ex.Message}" +
                    $"| stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericPagedResponse<TransactionDto>> MerchantAsync(PaginationParams paginationParams)
        {
            try
            {
                var transactionsQuery = await _transactionQuery.ListAsync(x => x.MerchantId == _merchantContext.MerchantId);
                if (!string.IsNullOrWhiteSpace(paginationParams.SearchTerm))
                {
                    var searchConfig = new SearchConfig<Transaction>
                    {
                        SearchTerm = paginationParams.SearchTerm,
                        SearchProperties = new List<SearchProperty<Transaction>>
                        {
                            new(){ PropertyExpression = x => x.Reference, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.Currency, SearchType = SearchType.Contains },
                            new(){ PropertyExpression = x => x.Amount, SearchType = SearchType.Contains },
                        }
                    };

                    transactionsQuery = transactionsQuery.DynamicSearch(searchConfig);
                }

                var result = await transactionsQuery.ToPagedResultAsync(paginationParams);
                var transactionsDto = _mapper.Map<PagedResult<TransactionDto>>(result);

                return GenericPagedResponse<TransactionDto>.Success(transactionsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to fecth all merchant transactions - {ex.Message}" +
                    $"| stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
