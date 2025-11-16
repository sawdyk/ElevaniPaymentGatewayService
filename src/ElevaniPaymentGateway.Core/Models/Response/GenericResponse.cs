using System.Net;

namespace ElevaniPaymentGateway.Core.Models.Response
{
    public class GenericResponse<T> : GenericResponse
    {
        public string? Message { get; set; }
        public HttpStatusCode Code { get; set; }
        public T? Data { get; set; }

        public static GenericResponse<T> Success(T data, string message = null)
        {
            return new GenericResponse<T>
            {
                Code = HttpStatusCode.OK,
                Message = (message ?? "Successful"),
                Data = data
            };
        }

        public static GenericResponse<T> BadRequest(T data, string message = null)
        {
            return new GenericResponse<T>
            {
                Code = HttpStatusCode.BadRequest,
                Message = (message ?? "BadRequest"),
                Data = data
            };
        }

        public static GenericResponse<T> InternalServerError(T data, string message = null)
        {
            return new GenericResponse<T>
            {
                Code = HttpStatusCode.InternalServerError,
                Message = (message ?? "InternalServerError"),
                Data = data
            };
        }

        public static GenericResponse<T> NotFound(T data, string message = null)
        {
            return new GenericResponse<T>
            {
                Code = HttpStatusCode.NotFound,
                Message = (message ?? "NotFound"),
                Data = data
            };
        }

        public static GenericResponse<T> NoContent(T data, string message = null)
        {
            return new GenericResponse<T>
            {
                Code = HttpStatusCode.NoContent,
                Message = (message ?? "NoContent"),
                Data = data
            };
        }
    }

    public class GenericResponse
    {
        public string? Message { get; set; }
        public HttpStatusCode Code { get; set; }

        public static GenericResponse Success(string message = null)
        {
            return new GenericResponse
            {
                Code = HttpStatusCode.OK,
                Message = (message ?? "Successful")
            };
        }

        public static GenericResponse BadRequest(string message = null)
        {
            return new GenericResponse
            {
                Code = HttpStatusCode.BadRequest,
                Message = (message ?? "BadRequest")
            };
        }

        public static GenericResponse InternalServerError(string message = null)
        {
            return new GenericResponse
            {
                Code = HttpStatusCode.InternalServerError,
                Message = (message ?? "InternalServerError")
            };
        }

        public static GenericResponse NoContent(string message = null)
        {
            return new GenericResponse
            {
                Code = HttpStatusCode.NoContent,
                Message = (message ?? "NoContent")
            };
        }

        public static GenericResponse NotFound(string message = null)
        {
            return new GenericResponse
            {
                Code = HttpStatusCode.NotFound,
                Message = (message ?? "NotFound")
            };
        }
    }
}
