using Contracts.Domains.Interfaces;

namespace Contracts.Domains.Implementations;

public abstract class EntityBase<TKey> : IEntityBase<TKey>
{
    public TKey Id { get; set; }
}