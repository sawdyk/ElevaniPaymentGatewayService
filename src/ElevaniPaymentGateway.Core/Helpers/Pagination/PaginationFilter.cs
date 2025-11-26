namespace ElevaniPaymentGateway.Core.Helpers.Pagination
{
    public class PaginationFilter
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }

        public PaginationFilter()
        {
            this.pageSize = 50;
            this.pageNumber = 1;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.pageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.pageSize = pageSize > 50 ? 50 : pageSize;
        }
    }
}
