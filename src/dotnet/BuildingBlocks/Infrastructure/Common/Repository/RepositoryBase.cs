using System.Linq.Expressions;
using Contracts.Common.Interfaces;
using Contracts.Domains;
using Contracts.Domains.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Common.Repository;

public class RepositoryBase<T, TKey, TContext> : RepositoryQueryBase<T, TKey, TContext>, IRepositoryBase<T, TKey, TContext>
    where T : EntityBase<TKey>
    where TContext : DbContext
{
    private readonly IUnitOfWork<TContext> _unitOfWork;
    private readonly TContext _dbContext;

    public RepositoryBase(IUnitOfWork<TContext> unitOfWork, TContext dbContext) : base(dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    

    public async Task<IDbContextTransaction> BeginTransactionAsync()
        => await _dbContext.Database.BeginTransactionAsync();

    public async Task EndTransactionAsync()
    {
        await SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync() 
        => await _dbContext.Database.RollbackTransactionAsync();

    public void Create(T entity) => _dbContext.Set<T>().Add(entity);

    public async Task<TKey> CreateAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        return entity.Id;
    }

    public IList<TKey> CreateList(IEnumerable<T> entities)
    {
        var entitiesList = entities.ToList();
        _dbContext.Set<T>().AddRange(entitiesList);
        return entitiesList.Select(x => x.Id).ToList();
    }

    public async Task<IList<TKey>> CreateListAsync(IEnumerable<T> entities)
    {
        var entitiesList = entities.ToList();
        await _dbContext.Set<T>().AddRangeAsync(entitiesList);
        return entitiesList.Select(e => e.Id).ToList();
    }

    public void Update(T entity) => _dbContext.Set<T>().Update(entity);

    public Task UpdateAsync(T entity)
    {
        if (_dbContext.Entry(entity).State is EntityState.Unchanged) return Task.CompletedTask;

        T? exist = _dbContext.Set<T>().Find(entity.Id);
        _dbContext.Entry(exist).CurrentValues.SetValues(entity);
        
        return Task.CompletedTask;
    }

    public void UpdateList(IEnumerable<T> entities) => _dbContext.Set<T>().UpdateRange(entities);

    public Task UpdateListAsync(IEnumerable<T> entities)
        => _dbContext.Set<T>().AddRangeAsync(entities);

    public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);

    public Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public void DeleteList(IEnumerable<T> entities) => _dbContext.RemoveRange(entities);

    public Task DeleteListAsync(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().RemoveRange(entities);
        return Task.CompletedTask;
    }

    public Task<int> SaveChangesAsync()
        => _unitOfWork.CommitAsync();
}