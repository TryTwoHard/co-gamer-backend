using System.Linq.Expressions;
using Contracts.Common.Interfaces;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Common.Repository;

public class RepositoryBaseAsync<T, TKey, TContext> : IRepositoryBaseAsync<T, TKey, TContext>
    where T : EntityBase<TKey>
    where TContext : DbContext
{
    private readonly IUnitOfWork<TContext> _unitOfWork;
    private readonly TContext _dbcontext;

    public RepositoryBaseAsync(IUnitOfWork<TContext> unitOfWork, TContext dbcontext)
    {
        _unitOfWork = unitOfWork;
        _dbcontext = dbcontext;
    }

    public IQueryable<T> GetAll(bool trackChanges = false)
        => !trackChanges ? _dbcontext.Set<T>().AsNoTracking()
                         : _dbcontext.Set<T>();

    public IQueryable<T> GetAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        var items = GetAll(trackChanges);
        items = includeProperties.Aggregate(items, 
                                (current, includeProperty) 
                                    => current.Include(includeProperty));
        
        return items;
        
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
        => !trackChanges ? _dbcontext.Set<T>().Where(expression).AsNoTracking()
                         : _dbcontext.Set<T>().Where(expression);

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        var items = FindByCondition(expression, trackChanges);
        items = includeProperties.Aggregate(items, 
                                (current, includeProperty) 
                                    => current.Include(includeProperty));

        return items;
    }

    public async Task<T?> GetByIdAsync(TKey id) 
        => await FindByCondition(x => x.Id.Equals(id))
            .FirstOrDefaultAsync();

    public async Task<T?> GetByIdAsync(TKey id, params Expression<Func<T, object>>[] includeProperties)
        => await FindByCondition(x => x.Id.Equals(id), trackChanges: false, includeProperties)
            .FirstOrDefaultAsync();

    public async Task<IDbContextTransaction> BeginTransactionAsync()
        => await _dbcontext.Database.BeginTransactionAsync();

    public async Task EndTransactionAsync()
    {
        await SaveChangesAsync();
        await _dbcontext.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync() 
        => await _dbcontext.Database.RollbackTransactionAsync();

    public async Task<TKey> CreateAsync(T entity)
    {
        await _dbcontext.Set<T>().AddAsync(entity);
        return entity.Id;
    }

    public async Task<IList<TKey>> CreateListAsync(IEnumerable<T> entities)
    {
        await _dbcontext.Set<T>().AddRangeAsync(entities);
        return entities.Select(e => e.Id).ToList();
    }

    public Task UpdateAsync(T entity)
    {
        if (_dbcontext.Entry(entity).State is EntityState.Unchanged) return Task.CompletedTask;

        T? exist = _dbcontext.Set<T>().Find(entity.Id);
        _dbcontext.Entry(exist).CurrentValues.SetValues(entity);
        
        return Task.CompletedTask;
    }

    public Task UpdateListAsync(IEnumerable<T> entities)
        => _dbcontext.Set<T>().AddRangeAsync(entities);

    public Task DeleteAsync(T entity)
    {
        _dbcontext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteListAsync(IEnumerable<T> entities)
    {
        _dbcontext.Set<T>().RemoveRange(entities);
        return Task.CompletedTask;
    }

    public Task<int> SaveChangesAsync()
        => _unitOfWork.CommitAsync();


}