using System.Linq.Expressions;
using Ardalis.Specification;
using Contracts.Common.Interfaces;
using Infrastructure.Common.Repository;
using Microsoft.EntityFrameworkCore;
using Tournament.API.Extensions;
using Tournament.API.Models.Entities;
using Tournament.API.Persistence;
using Tournament.API.Repositories.Interfaces;

namespace Tournament.API.Repositories.Implementations;

public class TournamentRepository : RepositoryBase<TournamentEntity, Guid, TournamentContext>, ITournamentRepository
{
    public TournamentRepository(IUnitOfWork<TournamentContext> unitOfWork, TournamentContext dbContext) : base(unitOfWork, dbContext)
    {
    }

    public async Task<IQueryable<TournamentEntity>> GetTournamentsAsync() => GetAll();

    public Task<TournamentEntity?> GetTournamentByIdAsync(Guid id) => GetByIdAsync(id);

    public Task CreateTournamentAsync(TournamentEntity tournament) => CreateAsync(tournament);

    public Task UpdateTournamentAsync(TournamentEntity tournament) => UpdateAsync(tournament);

    public async Task DeleteTournamentAsync(Guid id)
    {
        var tournament = await GetTournamentByIdAsync(id);
        await DeleteAsync(tournament!);
    }
}