using Contracts.Domains.Implementations;

namespace Infrastructure.Extensions;

public static class QueryExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> items, PaginationOptions paginationOptions)
    {
        return items.Skip(paginationOptions.Size * (paginationOptions.Page - 1))
            .Take(paginationOptions.Size);
    }
}