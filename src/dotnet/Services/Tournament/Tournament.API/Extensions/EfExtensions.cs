using System.Data.Entity.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Tournament.API.Extensions;

public static class EfExtensions {
    public static Task<List<TSource>> ToListAsyncSafe<TSource>(this IQueryable<TSource> source) {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (!(source is IDbAsyncEnumerable<TSource>))
            return Task.FromResult(source.ToList());
        return source.ToListAsync();
    }
}