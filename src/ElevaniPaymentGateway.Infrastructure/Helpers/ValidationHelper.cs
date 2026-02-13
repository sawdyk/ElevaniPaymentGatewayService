using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models.Request.TransactionService;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
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

            if (string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName) || string.IsNullOrEmpty(request.Email)
                || string.IsNullOrEmpty(request.Address) || string.IsNullOrEmpty(request.Country) || string.IsNullOrEmpty(request.City)
                || string.IsNullOrEmpty(request.State) || string.IsNullOrEmpty(request.Zip) || string.IsNullOrEmpty(request.PhoneNumber) 
                || string.IsNullOrEmpty(request.Currency) || string.IsNullOrEmpty(request.CardNumber) || string.IsNullOrEmpty(request.CardExpiryYear) 
                || string.IsNullOrEmpty(request.CardExpiryMonth) || string.IsNullOrEmpty(request.CardCVV)
                || string.IsNullOrEmpty(request.Reference) || string.IsNullOrEmpty(request.Description))
                throw new DataValidationException($"Contains one or more null or empty values");
          
            if (request.FirstName.Any(ch => !char.IsLetterOrDigit(ch)))
                throw new DataValidationException($"{nameof(request.FirstName)} contains special characters");
            if (request.LastName.Any(ch => !char.IsLetterOrDigit(ch)))
                throw new DataValidationException($"{nameof(request.LastName)} contains special characters");
            if (IsValidEmail(request.Email) is false)
                throw new DataValidationException($"{nameof(request.Email)} is not a valid email address"); 
            if (request.Country.Length > 3 || request.Country.Length < 2)
                throw new DataValidationException($"Invalid {nameof(request.Country)}");
            if (request.Amount <= 0)
                throw new DataValidationException($"Invalid {nameof(request.Amount)}");
            if (request.Reference.Length > 30)
                throw new DataValidationException($"{nameof(request.Reference)} cannot be more than 30 characters");
            if (request.Reference.Any(ch => !char.IsLetterOrDigit(ch)))
                throw new DataValidationException($"{nameof(request.Reference)} contains special characters");
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

        public bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }

        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
