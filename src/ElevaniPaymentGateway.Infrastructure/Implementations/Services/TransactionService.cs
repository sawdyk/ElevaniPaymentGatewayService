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
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.PaymentGateway.PayAgency;
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
        private readonly IPayAgencyPaymentService _payAgencyPaymentService;
        public TransactionService(ILogger<TransactionService> logger, ITransactionQuery transactionQuery, IMapper mapper, 
            IGratipPaymentService gratipPaymentService, IPaymentHttpContextHelperService paymentHttpContextHelperService, 
            IPayAgencyPaymentService payAgencyPaymentService)
        {
            _logger = logger;
            _transactionQuery = transactionQuery;
            _mapper = mapper;
            _gratipPaymentService = gratipPaymentService;
            _paymentHttpContextHelperService = paymentHttpContextHelperService;
            _merchantContext = _paymentHttpContextHelperService.MerchantContext();
            _payAgencyPaymentService = payAgencyPaymentService;
        }

        public async Task<GenericResponse<TransactionResponse>> InitiateTransactionViaPaymentGatewayAsync(TransactionRequest request)
        {
            try
            {
                TransactionResponse initiatePaymentResponse = new TransactionResponse();
                switch (_merchantContext.PaymentGateway)
                {
                    case nameof(PaymentGateways.GRATIP):
                        initiatePaymentResponse = await _gratipPaymentService.InitiateTransactionAsync(request);
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

        public async Task<GenericResponse<PATransactionResponse>> InitiateTransactionViaServerAsync(string encryptedRequest)
        {
            try
            {
                PATransactionResponse initiatePaymentResponse = new PATransactionResponse();
                switch (_merchantContext.PaymentGateway)
                {
                    case nameof(PaymentGateways.PAYAGENCY):
                        initiatePaymentResponse = await _payAgencyPaymentService.InitiateTransactionAsync(encryptedRequest);
                        break;

                    default:
                        break;
                }

                return GenericResponse<PATransactionResponse>.Success(initiatePaymentResponse);
            }
            catch (Exception ex)
            when (ex is NotFoundException || ex is GenericException
            || ex is DataValidationException || ex is ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to initiate payment via server to server >> " +
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
                var transactionsQuery = await _transactionQuery.ListAsync(x => x.MerchantId == merchantId, true);
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
                            new(){ PropertyExpression = x => x.GratipTransaction.TransactionReference, SearchType = SearchType.Contains },
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
