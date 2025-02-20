using System.Text.Json.Serialization;

namespace Shared.Pagination;

/// <summary>
/// Represents a paginated list of items.
/// </summary>
/// <typeparam name="T">The type of items in the paginated list.</typeparam>
public class PagedList<T>
{
    [JsonPropertyName("items")]
    public List<T> Items { get; } // Gets the list of items in the current page.
    [JsonPropertyName("page")] 
    public int Page { get; } // Gets the current page number.
    [JsonPropertyName("pageSize")]
    public int PageSize { get; } // Gets the number of items per page.
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; } // Gets the total count of items across all pages.
    [JsonPropertyName("hasPreviousPage")]
    public bool HasPreviousPage => Page > 1; // Indicates whether there is a previous page.
    [JsonPropertyName("hasNextPage")]
    public bool HasNextPage => Page * PageSize < TotalCount; // Indicates whether there is a next page.

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
    /// </summary>
    /// <param name="items">The items for the current page.</param>
    /// <param name="page">The current page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="totalCount">The total number of items.</param>
    public PagedList(List<T> items, int page, int pageSize, int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
}