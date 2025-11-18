using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models.Request.TransactionService;
using Microsoft.Extensions.Logging;
using System.Text;

namespace ElevaniPaymentGateway.Infrastructure.Helpers
{
    public class ValidationHelper
    {
        private readonly ILogger<ValidationHelper> _logger;
        public ValidationHelper(ILogger<ValidationHelper> logger)
        {
            _logger = logger;
        }

        public void ValidateRequest(TransactionRequest request)
        {
            List<string> validCurrencies = new List<string>() { "USD", "GBP", "EUR" };
            var errors = new StringBuilder();

            if (request.Amount <= 0)
                errors.AppendLine($"{nameof(request.Amount)} is invalid");

            if (string.IsNullOrEmpty(request.Currency))
                errors.AppendLine($"{nameof(request.Currency)} cannot be null or empty");

            if (string.IsNullOrEmpty(request.Reference))
                errors.AppendLine($"{nameof(request.Reference)} cannot be null or empty");

            if (!validCurrencies.Contains(request.Currency))
                errors.AppendLine($"Invalid {nameof(request.Currency)}");

            if(errors.Length > 0)
                throw new DataValidationException(errors.ToString(), errors);
        }
    }
}
