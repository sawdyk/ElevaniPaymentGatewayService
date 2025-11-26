using System.Net;
using System.Runtime.Serialization;

namespace ElevaniPaymentGateway.Core.Exceptions
{
    public class UnhandledException : BaseException
    {
        public UnhandledException(object? details = null)
            : base("Your request wasn't completed. Please, try again.", details)
        {
        }

        public UnhandledException(string message, object? details = null)
            : base(message, details)
        {
        }

        public UnhandledException(string message, Exception inner, object? details = null)
            : base(message, inner, details)
        {
        }

        protected UnhandledException(SerializationInfo info, StreamingContext context, object? details = null)
            : base(info, context, details)
        {
        }

        public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
    }
}
