using System.Linq.Expressions;
using Ardalis.Specification;
using Contracts.Common.Interfaces;
using Tournament.API.Models.Entities;
using Tournament.API.Persistence;

namespace Tournament.API.Repositories.Interfaces;

public interface ITournamentRepository : IRepositoryBase<TournamentEntity, Guid, TournamentContext>
{
    Task<IEnumerable<TournamentEntity>> GetTournamentsAsync(ISpecification<TournamentEntity> specification);
    Task<TournamentEntity?> GetTournamentByIdAsync(Guid id);
    Task CreateTournamentAsync(TournamentEntity tournament);
    Task UpdateTournamentAsync(TournamentEntity tournament);
    Task DeleteTournamentAsync(Guid id);
}