using Shared.Pagination;
using Shared.Sorting;

namespace Application.DTOs;

// Query object for filtering, sorting, and paginating flight data
public class FlightQuery : IPagedQuery, ISortedQuery
{
    public int Page { get; set; } = 1; // Current page number for pagination (default: 1)
    public int PageSize { get; set; } = 10; // Number of records per page (default: 10)
    public string? SortColumn { get; set; } // Column name to sort by (optional)
    public SortOrder? SortOrder { get; set; } // Sorting order (Ascending or Descending) (optional)
}