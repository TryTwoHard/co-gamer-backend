using Microsoft.EntityFrameworkCore;

namespace Contracts.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    
}

public interface IUnitOfWork<TContext> : IUnitOfWork
    where TContext : DbContext
{
    public Task<int> CommitAsync();
}