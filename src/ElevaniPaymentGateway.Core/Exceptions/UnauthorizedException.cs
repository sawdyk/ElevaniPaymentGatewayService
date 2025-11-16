using System.Net;
using System.Runtime.Serialization;

namespace ElevaniPaymentGateway.Core.Exceptions
{
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(object? details = null)
            : base("Unauthorized access", details)
        {
        }

        public UnauthorizedException(string message, object? details = null)
            : base(message, details)
        {
        }

        public UnauthorizedException(string message, Exception inner, object? details = null)
            : base(message, inner, details)
        {
        }

        protected UnauthorizedException(SerializationInfo info, StreamingContext context, object? details = null)
            : base(info, context, details)
        {
        }

        public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
    }
}
