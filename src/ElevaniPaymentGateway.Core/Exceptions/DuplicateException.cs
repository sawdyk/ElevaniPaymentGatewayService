using System.Net;
using System.Runtime.Serialization;

namespace ElevaniPaymentGateway.Core.Exceptions
{
    public class DuplicateException : BaseException
    {
        public DuplicateException(object? details = null) : base("Item already exists", details)
        {
        }

        public DuplicateException(string message, object? details = null)
            : base(message, details)
        {
        }

        public DuplicateException(string message, Exception inner, object? details = null)
            : base(message, inner, details)
        {
        }

        protected DuplicateException(SerializationInfo info, StreamingContext context, object? details = null)
            : base(info, context, details)
        {
        }

        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    }
}
