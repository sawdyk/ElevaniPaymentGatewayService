using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Helpers;
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
                throw new DataValidationException($"Invalid {nameof(request.Amount)}");

            if (string.IsNullOrEmpty(request.Currency))
                throw new DataValidationException($"{nameof(request.Currency)} cannot be null or empty");

            if (string.IsNullOrEmpty(request.CountryCode))
                throw new DataValidationException($"{nameof(request.CountryCode)} cannot be null or empty");

            if (string.IsNullOrEmpty(request.Reference))
                throw new DataValidationException($"{nameof(request.Reference)} cannot be null or empty");

            if (request.CountryCode.Length > 2)
                throw new DataValidationException($"Invalid {nameof(request.CountryCode)}");

            if (request.Reference.Length > 30)
                throw new DataValidationException($"{nameof(request.Reference)} cannot be more than 30 characters");

            if (request.Reference.Any(ch => !char.IsLetterOrDigit(ch)))
                throw new DataValidationException($"{nameof(request.Reference)} contains special characters");

            if (request.Description.Length > 500)
                throw new DataValidationException($"{nameof(request.Description)} is exceeded maximum characters");

            if (!validCurrencies.Contains(request.Currency))
                throw new DataValidationException($"Invalid {nameof(request.Currency)}");
        }

        public void ValidateRequest(PATransactionRequest request)
        {
            List<string> validCurrencies = new List<string>() { "USD", "GBP", "EUR" };
            var errors = new StringBuilder();

            if (request.FirstName.Any(ch => !char.IsLetterOrDigit(ch)))
                throw new DataValidationException($"{nameof(request.FirstName)} contains special characters");
            if (request.LastName.Any(ch => !char.IsLetterOrDigit(ch)))
                throw new DataValidationException($"{nameof(request.LastName)} contains special characters");
            if (request.Country.Length > 3 || request.Country.Length < 3)
                throw new DataValidationException($"Invalid {nameof(request.Country)}");
            if (request.Amount <= 0)
                throw new DataValidationException($"Invalid {nameof(request.Amount)}");
            if (request.Reference.Length > 30)
                throw new DataValidationException($"{nameof(request.Reference)} cannot be more than 30 characters");
            if(!StringHelpers.IsValidIPv4Address(request.IPAddress.Trim()))
                throw new DataValidationException($"{nameof(request.IPAddress)} is invalid");
            if (request.CardExpiryMonth.Length > 2 || request.CardExpiryMonth.Length < 2)
                throw new DataValidationException($"Invalid {nameof(request.CardExpiryMonth)}");
            if (request.CardExpiryYear.Length > 4 || request.CardExpiryYear.Length < 4)
                throw new DataValidationException($"Invalid {nameof(request.CardExpiryYear)}");
            if (request.CardCVV.Length > 3 || request.CardCVV.Length < 3)
                throw new DataValidationException($"Invalid {nameof(request.CardCVV)}");
            if (request.Description.Length > 500)
                throw new DataValidationException($"{nameof(request.Description)} is exceeded maximum characters");
            if (!validCurrencies.Contains(request.Currency))
                throw new DataValidationException($"Invalid {nameof(request.Currency)}");
        }
    }
}
