using System.Linq.Expressions;
using Contracts.Common.Interfaces;
using Contracts.Domains.Implementations;
using Infrastructure.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Repository;

public class RepositoryQueryBase<T, TKey, TContext> : IRepositoryQueryBase<T, TKey, TContext>
    where T : EntityBase<TKey>
    where TContext : DbContext
{
    private readonly TContext _dbContext;

    public RepositoryQueryBase(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<T> GetAll(bool trackChanges = false)
        => !trackChanges ? _dbContext.Set<T>().AsNoTracking()
            : _dbContext.Set<T>();

    public IQueryable<T> GetAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        var items = GetAll(trackChanges);
        items = includeProperties.Aggregate(items, 
            (current, includeProperty) 
                => current.Include(includeProperty));
        
        return items;
    }
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
        => !trackChanges ? _dbContext.Set<T>().Where(expression).AsNoTracking()
            : _dbContext.Set<T>().Where(expression);

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties)
    {
        var items = FindByCondition(expression, trackChanges);
        items = includeProperties.Aggregate(items,
            (current, includeProperty)
                => current.Include(includeProperty));

        return items;
    }

    public async Task<T?> GetByIdAsync(TKey id) 
        => await FindByCondition(x => x.Id!.Equals(id))
            .FirstOrDefaultAsync();

    public async Task<T?> GetByIdAsync(TKey id, params Expression<Func<T, object>>[] includeProperties)
        => await FindByCondition(x => x.Id!.Equals(id), trackChanges: false, includeProperties)
            .FirstOrDefaultAsync();
}