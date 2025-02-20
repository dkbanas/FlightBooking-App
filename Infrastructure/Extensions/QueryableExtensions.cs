using System.Linq.Expressions;

namespace Shared.Pagination;

/// <summary>
/// Provides extension methods for <see cref="IQueryable{T}"/> to enable dynamic ordering.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Orders the elements of a sequence dynamically by the specified property name.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The source queryable collection to order.</param>
    /// <param name="propertyName">The name of the property to order by.</param>
    /// <param name="descending">Indicates whether to order in descending order.</param>
    /// <returns>An <see cref="IQueryable{T}"/> that contains the ordered elements.</returns>
    public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string propertyName, bool descending)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.PropertyOrField(parameter, propertyName);
        var lambda = Expression.Lambda(property, parameter);

        string methodName = descending ? "OrderByDescending" : "OrderBy";
        var result = typeof(Queryable).GetMethods()
            .First(method => method.Name == methodName && method.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.Type)
            .Invoke(null, new object[] { source, lambda });

        return (IQueryable<T>)result!;
    }
}