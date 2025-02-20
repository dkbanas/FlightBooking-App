namespace Shared.Pagination;

/// <summary>
/// Interface for paged queries.
/// </summary>
public interface IPagedQuery
{
    int Page { get; set; } // Gets or sets the page number.
    int PageSize { get; set; } // Gets or sets the number of items per page.
}