using ElevaniPaymentGateway.Core.Helpers.Pagination;

namespace ElevaniPaymentGateway.Core.Models.Response
{
    public class ReportResponse<T>
    {
        public PagedResult<T> Data { get; set; }
        public string FileName { get; set; }
        public string Base64Data { get; set; }
    }
}
