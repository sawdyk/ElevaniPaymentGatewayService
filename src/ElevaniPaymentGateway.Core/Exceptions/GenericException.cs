using System.Net;
using System.Runtime.Serialization;

namespace ElevaniPaymentGateway.Core.Exceptions
{
    public class GenericException : BaseException
    {
        public GenericException(object? details = null) : base("An error occurred", details)
        {
        }

        public GenericException(string message, object? details = null)
            : base(message, details)
        {
        }

        public GenericException(string message, Exception inner, object? details = null)
            : base(message, inner, details)
        {
        }

        protected GenericException(SerializationInfo info, StreamingContext context, object? details = null)
            : base(info, context, details)
        {
        }

        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    }
}
