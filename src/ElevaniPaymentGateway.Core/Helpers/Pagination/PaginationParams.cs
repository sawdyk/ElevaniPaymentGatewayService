namespace ElevaniPaymentGateway.Core.Helpers.Pagination
{
    public class PaginationParams
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;
        private int _pageNumber = 1;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        //public string? SortBy { get; set; }
        //public bool SortDescending { get; set; }
        public string? SearchTerm { get; set; }
    }

    public class PagedResult<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public IEnumerable<T> Data { get; set; }
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
    }
}
