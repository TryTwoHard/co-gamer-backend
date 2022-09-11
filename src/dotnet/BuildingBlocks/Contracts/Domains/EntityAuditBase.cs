using Contracts.Domains.Interfaces;

namespace Contracts.Domains.Implementations;

public abstract class EntityAuditBase<TKey> : EntityBase<TKey>, IAuditable
{
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
}