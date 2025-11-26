using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Exceptions;
using Microsoft.Data.SqlClient;

namespace ElevaniPaymentGateway.Core.Helpers
{
    public static class ExceptionHandler
    {
        public static Exception HandleExceptions(Exception exception)
        {
            if (exception.InnerException is SqlException sqlException)
            {
                if (sqlException.Number == 2601) //
                    throw new DuplicateException("Duplicate record exist");
                if (sqlException.Number == 547) //foreign key constraint exception
                    throw new GenericException("Foreign key constraint exception");
            }
            if (exception is NullReferenceException)
                throw new GenericException("Null reference exception");
            if (exception is ArgumentNullException)
                throw new GenericException("Argument null exception");

            throw new UnhandledException(RespMsgConstants.UnhandledException);
        }
    }
}
