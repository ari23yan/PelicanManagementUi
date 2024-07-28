namespace PelicanManagementUi.Models.ViewModels.Common.Pagination
{
    public class PaginationMetadata<T>
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public bool HasNextPage => CurrentPage < TotalPages;
        public bool HasPreviousPage => CurrentPage > 1;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public List<T> Data { get; set; }
    }
}
