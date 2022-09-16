using System.Linq.Expressions;
using AutoMapper;
using Contracts.Domains.Implementations;
using Microsoft.EntityFrameworkCore;
using Tournament.API.Models.DTOs;
using Tournament.API.Models.Entities.Tournament;
using Tournament.API.Models.Statuses;
using Tournament.API.Repositories.Interfaces;
using Tournament.API.Services.Interfaces.Tournament;

namespace Tournament.API.Services.Implementations.Tournament;

public class TournamentService : ITournamentService
{
    private readonly ITournamentRepository _repository;
    private readonly IMapper _mapper;

    public TournamentService(ITournamentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TournamentDTO>> GetTournaments()
    {
        var list = await _repository.GetAll().ToListAsync();
        return _mapper.Map<IEnumerable<TournamentDTO>>(list);
    }

    public async Task<TournamentDTO?> GetTournamentById(Guid id)
    {
        var tournament = await _repository.GetTournamentByIdAsync(id);
        return _mapper.Map<TournamentDTO>(tournament);
    }

    public async Task CreateNewTournament(TournamentDTO tournament)
    {
        var tournamentEntity = _mapper.Map<TournamentEntity>(tournament);
        tournamentEntity.Status = TournamentStatus.Draft;
        await _repository.CreateTournamentAsync(tournamentEntity);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateTournament(TournamentDTO tournament)
    {
        var tournamentEntity = _mapper.Map<TournamentEntity>(tournament);
        await _repository.UpdateTournamentAsync(tournamentEntity);
        await _repository.SaveChangesAsync();
    }

    public async Task CancelTournament(Guid id)
    {
        var tournament = await _repository.GetTournamentByIdAsync(id);
        tournament!.Status = TournamentStatus.Canceled;
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteTournament(Guid id)
    {
        await _repository.DeleteTournamentAsync(id);
        await _repository.SaveChangesAsync();
    }

    public async Task<bool> ValidateTournamentToPublish(Guid tournamentId)
    {
        return await ValidateTournament(tournamentId);
    }

    private async Task<bool> ValidateTournament(Guid tournamentId)
    {
        // check for current tournament
        var tournament = await _repository.GetTournamentByIdAsync(tournamentId);
        if (tournament is null)
        {
            throw new NullReferenceException("Tournament not drafted for validation!");
        }
        
        //
        return true;
    }
    
    public async Task PublishTournament(Guid tournamentId)
    {
        var tournament = await GetTournamentById(tournamentId);
        var valid = await ValidateTournamentToPublish(tournamentId);
        if (valid)
        {
            tournament!.Status = TournamentStatus.Publish;
            await _repository.SaveChangesAsync();
        }
    }
}