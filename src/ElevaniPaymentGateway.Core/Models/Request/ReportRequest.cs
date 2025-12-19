using ElevaniPaymentGateway.Core.Enums;

namespace ElevaniPaymentGateway.Core.Models.Request
{
    public class ReportRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class TransactionReportRequest : ReportRequest
    {
        public string? MerchantId { get; set; }
        public TransactionStatus? Status { get; set; }
    }
}
