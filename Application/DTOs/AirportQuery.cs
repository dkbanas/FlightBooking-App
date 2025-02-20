using Shared.Pagination;
using Shared.Sorting;

namespace Application.DTOs;

// Query object for filtering, sorting, and paginating airport data
public class AirportQuery : IPagedQuery, ISortedQuery
{
    public int Page { get; set; } // Current page number for pagination
    public int PageSize { get; set; } // Number of records per page
    public string? SortColumn { get; set; } // Column name to sort by
    public SortOrder? SortOrder { get; set; } // Sorting order (Ascending or Descending)
    public string? Continent { get; set; } // Filter by continent (optional)
}