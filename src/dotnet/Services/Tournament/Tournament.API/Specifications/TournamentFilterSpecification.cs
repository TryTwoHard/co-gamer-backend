using Ardalis.Specification;
using Tournament.API.Extensions;
using Tournament.API.Models.Entities;

namespace Tournament.API.Specifications;

public class TournamentFilterSpecification : Specification<TournamentEntity>
{
    public TournamentFilterSpecification(int? status, Guid? hostId, Guid? gameId)
    {
        status = status ?? 0;
        Query.Where(x => x.GetStatusIndex().Equals(status) &&
                         (hostId == x.HostId || !hostId.HasValue) &&
                         (gameId == x.GameCategory.Id || !gameId.HasValue));
    }
}