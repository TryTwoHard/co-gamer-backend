using Contracts.Common.Interfaces;
using Tournament.API.Models.Entities;
using Tournament.API.Persistence;

namespace Tournament.API.Repositories.Interfaces;

public interface ITournamentRepository : IRepositoryBase<TournamentEntity, Guid, TournamentContext>
{
    Task<IQueryable<TournamentEntity>> GetTournamentsAsync();
    Task<TournamentEntity?> GetTournamentByIdAsync(Guid id);
    Task CreateTournamentAsync(TournamentEntity tournament);
    Task UpdateTournamentAsync(TournamentEntity tournament);
    Task DeleteTournamentAsync(Guid id);
}