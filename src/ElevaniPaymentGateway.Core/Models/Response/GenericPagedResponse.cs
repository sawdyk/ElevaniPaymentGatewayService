using ElevaniPaymentGateway.Core.Helpers.Pagination;
using System.Net;

namespace ElevaniPaymentGateway.Core.Models.Response
{
    public class GenericPagedResponse<T> where T : class
    {
        public HttpStatusCode Code { get; set; }
        public string? Message { get; set; }
        public PagedResult<T>? Result { get; set; }


        public static GenericPagedResponse<T> Success(PagedResult<T> data, string message = null)
        {
            return new GenericPagedResponse<T>
            {
                Code = HttpStatusCode.OK,
                Message = (message ?? "Successful"),
                Result = data
            };
        }

        public static GenericPagedResponse<T> InternalServerError(PagedResult<T> data, string message = null)
        {
            return new GenericPagedResponse<T>
            {
                Code = HttpStatusCode.InternalServerError,
                Message = (message ?? "InternalServerError"),
                Result = data
            };
        }

        public static GenericPagedResponse<T> NoContent(PagedResult<T> data, string message = null)
        {
            return new GenericPagedResponse<T>
            {
                Code = HttpStatusCode.NoContent,
                Message = (message ?? "NoContent"),
                Result = data
            };
        }

    }
    public class PagedResponse<T> where T : class
    {
        public IEnumerable<T>? Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public double TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public PagedResponse(int pageNumber, int pageSize, List<T> data)
        {
            int totalRecords = data.Count();
            if (pageNumber > 0 && pageSize > 0)
            {
                var paginationFilter = new PaginationFilter(pageNumber, pageSize);
                PageNumber = paginationFilter.pageNumber;
                PageSize = paginationFilter.pageSize;
                double totalPages = ((double)totalRecords / (double)paginationFilter.pageSize);
                int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
                TotalPages = roundedTotalPages;
                TotalRecords = totalRecords;
                Data = data.Skip((paginationFilter.pageNumber - 1) * paginationFilter.pageSize).Take(paginationFilter.pageSize);
            }
            else
            {
                TotalRecords = totalRecords;
                Data = data;
            }
        }
    }
}
