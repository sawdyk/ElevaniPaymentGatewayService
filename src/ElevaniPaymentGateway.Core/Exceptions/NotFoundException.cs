using System.Net;
using System.Runtime.Serialization;

namespace ElevaniPaymentGateway.Core.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(object? details = null) : base("Sorry, looks like what you're trying to find isn't here.", details)
        {
        }

        public NotFoundException(string message, object? details = null)
            : base(message, details)
        {
        }

        public NotFoundException(string message, Exception inner, object? details = null)
            : base(message, inner, details)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context, object? details = null)
            : base(info, context, details)
        {
        }

        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    }
}
