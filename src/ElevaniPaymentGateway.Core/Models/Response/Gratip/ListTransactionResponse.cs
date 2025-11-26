using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevaniPaymentGateway.Core.Models.Response.Gratip
{
    public class ListTransactionResponse
    {
        public bool success { get; set; }
        public string? message { get; set; }
        public ListTransactionResponseData? data { get; set; }
    }

    public class ListTransactionResponseData
    {
        public List<ListTransactionResponseCollection>? collections { get; set; }
        public ListTransactionResponsePagination? pagination { get; set; }
    }

    public class ListTransactionResponsePagination
    {
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public int totalItems { get; set; }
        public int limit { get; set; }
        public bool hasNext { get; set; }
        public bool hasPrev { get; set; }
    }

    public class ListTransactionResponseCollection
    {
        public int collection_id { get; set; }
        public string? request_reference { get; set; }
        public string? transaction_reference { get; set; }
        public string? external_reference { get; set; }
        public decimal amount { get; set; }
        public string? currency { get; set; }
        public string? status { get; set; }
        public Fees? fees { get; set; }
        public DateTime? created_at { get; set; }
    }
}
