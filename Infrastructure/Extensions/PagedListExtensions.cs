using Microsoft.EntityFrameworkCore;

namespace Shared.Pagination;

/// <summary>
/// Provides extension methods for working with paged lists.
/// </summary>
public static class PagedListExtensions<T>
{
    /// <summary>
    /// Creates a <see cref="PagedList{T}"/> asynchronously from a given <see cref="IQueryable{T}"/>.
    /// </summary>
    /// <param name="source">The source queryable collection to paginate.</param>
    /// <param name="page">The current page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="cancellationToken">An optional cancellation token for the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation, with a <see cref="PagedList{T}"/> result.</returns>
    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int page, int pageSize,
        CancellationToken? cancellationToken = null)
    {
        cancellationToken ??= CancellationToken.None;

        var totalCount = await source.CountAsync(cancellationToken.Value);
        var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken.Value);
            
        return new PagedList<T>(items, page, pageSize, totalCount);
    }
}