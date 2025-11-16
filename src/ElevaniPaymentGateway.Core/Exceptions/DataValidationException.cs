using System.Net;
using System.Runtime.Serialization;

namespace ElevaniPaymentGateway.Core.Exceptions
{
    public class DataValidationException : BaseException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public DataValidationException(object? details = null)
            : base("Sorry, we couldn't make that happen. Please, try again.", details)
        {
        }

        public DataValidationException(string message, object? details = null)
            : base(message, details)
        {
        }

        public DataValidationException(string message, Exception inner, object? details = null)
            : base(message, inner, details)
        {
        }

        protected DataValidationException(SerializationInfo info, StreamingContext context, object? details = null)
            : base(info, context, details)
        {
        }
    }
}
