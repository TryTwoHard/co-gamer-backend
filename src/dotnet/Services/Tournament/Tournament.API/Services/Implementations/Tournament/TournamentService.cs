using System.Linq.Expressions;
using AutoMapper;
using Contracts.Domains.Implementations;
using Microsoft.EntityFrameworkCore;
using Tournament.API.Controllers.Payloads.Requests;
using Tournament.API.Exceptions;
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

    public async Task<TournamentDTO> CreateNewTournament(DraftTournamentRequest tournament)
    {
        var tournamentEntity = _mapper.Map<TournamentEntity>(tournament);
        tournamentEntity.Status = TournamentStatus.Draft;
        await _repository.CreateTournamentAsync(tournamentEntity);
        await _repository.SaveChangesAsync();
        
        var tournamentToReturn = _mapper.Map<TournamentDTO>(tournamentEntity);
        return tournamentToReturn;
    }

    public async Task<TournamentDTO> UpdateTournament(Guid id, UpdateTournamentRequest tournamentRequest)
    {
        var tournament = await _repository.GetTournamentByIdAsync(id);
        if (tournament is null)
        {
            throw new TournamentNotFoundException($"Tournament with id {id} does not exist.");
        }
        
        var tournamentEntity = _mapper.Map<TournamentEntity>(tournamentRequest);
        await _repository.UpdateTournamentAsync(tournamentEntity);
        await _repository.SaveChangesAsync();

        var tournamentToReturn = _mapper.Map<TournamentDTO>(tournamentEntity);
        return tournamentToReturn;
    }

    public async Task<TournamentDTO> CancelTournament(Guid id)
    {
        // var tournament = await _repository.GetTournamentByIdAsync(id);
        // if (tournament is null)
        // {
        //     throw new TournamentNotFoundException($"Tournament with id {id} does not exist.");
        // }
        // tournament!.Status = TournamentStatus.Canceled;
        // await _repository.SaveChangesAsync();
        throw new TournamentNotFoundException($"Tournament with id {id} does not exist.");
        var canceledTournament = await GetTournamentById(id);
        return _mapper.Map<TournamentDTO>(canceledTournament);
    }

    public async Task<TournamentDTO> DeleteTournament(Guid id)
    {
        var tournament = await _repository.GetTournamentByIdAsync(id);
        if (tournament is null)
        {
            throw new TournamentNotFoundException($"Tournament with id {id} does not exist.");
        }
        await _repository.DeleteTournamentAsync(id);
        await _repository.SaveChangesAsync();

        var tournamentToReturn = _mapper.Map<TournamentDTO>(tournament);
        return tournamentToReturn;
    }

    public async Task<bool> ValidateTournamentToPublish(Guid id)
    {
        return await ValidateTournament(id);
    }

    private async Task<bool> ValidateTournament(Guid tournamentId)
    {
        //
        return true;
    }
    
    public async Task<TournamentDTO> PublishTournament(Guid id)
    {
        var tournament = await GetTournamentById(id);
        if (tournament is null)
        {
            throw new TournamentNotFoundException($"Tournament with id {id} does not exist.");
        }
        
        var valid = await ValidateTournamentToPublish(id);
        if (valid)
        {
            tournament!.Status = TournamentStatus.Publish;
            await _repository.SaveChangesAsync();
            var tournamentToReturn = _mapper.Map<TournamentDTO>(tournament);
            return tournamentToReturn;
        }
        else
        {
            throw new Exception();
        }
    }
}