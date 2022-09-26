using Ardalis.Specification;
using Tournament.API.Extensions;
using Tournament.API.Models.Entities;

namespace Tournament.API.Specifications;

public class TournamentFilterPaginatedSpecification : Specification<TournamentEntity>
{
    public TournamentFilterPaginatedSpecification(int skip, int take, int? status, Guid? hostId, Guid? gameId)
    {
        if (take == 0)
        {
            take = int.MaxValue;
        }

        Query.Where(x => x.GetStatusIndex().Equals(status) &&
                         (hostId == x.HostId || !hostId.HasValue) &&
                         (gameId == x.GameCategory.Id || !gameId.HasValue))
            .Skip(skip)
            .Take(take);
    }
}