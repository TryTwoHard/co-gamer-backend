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
        
        var now = DateTimeOffset.UtcNow;

        if (status == 1)
        {
            Query.Where(x => x.BeginTime > now);
        }
        else if (status == -1)
        {
            Query.Where(x => x.EndTime < now);
        }
        else if (status == 0)
        {
            Query.Where(x => x.BeginTime <= now && x.EndTime >= now);
        }
        

        Query.Where(x => (hostId == x.HostId || !hostId.HasValue) &&
                         (gameId == x.GameCategory.Id || !gameId.HasValue))
            .Skip(skip)
            .Take(take);
    }
}