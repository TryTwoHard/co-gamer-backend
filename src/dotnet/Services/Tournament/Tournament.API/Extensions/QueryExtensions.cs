using System.Linq.Expressions;
using Tournament.API.Controllers.Tournaments;
using Tournament.API.Models.Entities;

namespace Tournament.API.Extensions;

public static class QueryExtensions
{
    public static IQueryable<TEntity> OrderByCustom<TEntity>(this IQueryable<TEntity> items, string sortBy,
        string sortOrder)
    {
        var type = typeof(TEntity);
        var expression2 = Expression.Parameter(type, "t");
        var property = type.GetProperty(sortBy);
        var expression1 = Expression.MakeMemberAccess(expression2, property);
        var lambda = Expression.Lambda(expression1, expression2);
        var result = Expression.Call(
            typeof(Queryable),
            sortOrder == "desc" ? "OrderByDescending" : "OrderBy",
            new Type[] {type, property.PropertyType},
            items.Expression,
            Expression.Quote(lambda));

        return items.Provider.CreateQuery<TEntity>(result);
    }

    public static IQueryable<TournamentEntity> Paginate(this IQueryable<TournamentEntity> tournaments, TournamentQueryParameters parameters)
    {
        if (parameters.BeginDate is not null)
        {
            tournaments = tournaments.Where(t => t.BeginTime >= parameters.BeginDate);
        }
        
        if (parameters.EndDate is not null)
        {
            tournaments = tournaments.Where(t => t.EndTime <= parameters.EndDate);
        }

        if (!string.IsNullOrEmpty(parameters.Name))
        {
            tournaments = tournaments.Where(t => t.Name.Contains(parameters.Name));
        }

        if (!string.IsNullOrEmpty(parameters.Status))
        {
            tournaments = tournaments.Where(t => t.Status.Equals(parameters.Status));
        }

        if (!string.IsNullOrEmpty(parameters.SortBy))
        {
            tournaments = tournaments.OrderByCustom(parameters.SortBy, parameters.SortOrder);
        }

        tournaments = tournaments.Skip(parameters.Size * (parameters.Page - 1))
            .Take(parameters.Size);

        return tournaments;
    }

}