using AutoMapper;
using Microsoft.OpenApi.Extensions;
using Tournament.API.Controllers.Payloads;
using Tournament.API.Controllers.Tournaments;
using Tournament.API.Exceptions;
using Tournament.API.Extensions;
using Tournament.API.Helpers;
using Tournament.API.Models.DTOs;
using Tournament.API.Models.Entities;
using Tournament.API.Models.Statuses;
using Tournament.API.Repositories.Interfaces;
using Tournament.API.Services.Interfaces;

namespace Tournament.API.Services.Implementations;

public class TournamentService : ITournamentService
{
    private readonly ITournamentRepository _repository;
    private readonly IMapper _mapper;

    public TournamentService(ITournamentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Page<TournamentDTO>> GetTournaments(TournamentQueryParameters parameters)
    {
        var tournaments = await _repository.GetTournamentsAsync();
        tournaments.Paginate(parameters);

        var result = _mapper.Map<List<TournamentDTO>>(tournaments.ToList());
        var page = new Page<TournamentDTO>(result.Count) { Content = result};

        return page;
    }

    public async Task<TournamentDTO?> GetTournamentById(Guid id)
    {
        var tournament = await _repository.GetTournamentByIdAsync(id);
        if (tournament is null)
        {
            throw new TournamentNotFoundException($"Tournament with id {id} does not exist.");
        }
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
        tournamentEntity.Id = id;
        await _repository.UpdateTournamentAsync(tournamentEntity);
        await _repository.SaveChangesAsync();

        var tournamentToReturn = _mapper.Map<TournamentDTO>(tournamentEntity);
        return tournamentToReturn;
    }

    public async Task<TournamentDTO> CancelTournament(Guid id)
    {
        var tournament = await _repository.GetTournamentByIdAsync(id);
        if (tournament is null)
        {
            throw new TournamentNotFoundException($"Tournament with id {id} does not exist.");
        }

        if (tournament.Status is not TournamentStatus.Publish)
        {
            throw new BadHttpRequestException("Cannot cancel an unpublished tournament.");
        }
        
        tournament!.Status = TournamentStatus.Cancel;
        await _repository.SaveChangesAsync();
        var canceledTournament = await _repository.GetTournamentByIdAsync(id);
        return _mapper.Map<TournamentDTO>(canceledTournament);
    }

    public async Task<TournamentDTO> DeleteTournament(Guid id)
    {
        var tournament = await _repository.GetTournamentByIdAsync(id);
        if (tournament is null)
        {
            throw new TournamentNotFoundException($"Tournament with id {id} does not exist.");
        }

        if (tournament.Status is not TournamentStatus.Draft)
        {
            throw new TournamentInvalidException($"Cannot delete an {tournament.Status}ed tournament");
        }

        await _repository.DeleteTournamentAsync(id);
        await _repository.SaveChangesAsync();

        var tournamentToReturn = _mapper.Map<TournamentDTO>(tournament);
        return tournamentToReturn;
    }

    public async Task<TournamentDTO> ValidateTournamentToPublish(Guid id)
    {
        var validatedTournament = await ValidateTournament(id);
        return validatedTournament;
    }

    private async Task<TournamentDTO> ValidateTournament(Guid id)
    {
        var tournament = await _repository.GetTournamentByIdAsync(id);
        var now = DateTimeOffset.UtcNow;
        
        if (tournament!.EndTime <= now)
        {
            throw new TournamentInvalidException("Tournament's end time cannot be in the past");
        }
        
        if (tournament!.BeginTime <= now)
        {
            throw new TournamentInvalidException($"Tournament's begin time cannot be in the past");
        }
        
        return _mapper.Map<TournamentDTO>(tournament);
    }
    
    public async Task<TournamentDTO> PublishTournament(Guid id)
    {
        var tournament = await _repository.GetTournamentByIdAsync(id);
        if (tournament is null)
        {
            throw new TournamentNotFoundException($"Tournament with id {id} does not exist.");
        }
        
        await ValidateTournamentToPublish(id);
        tournament!.Status = TournamentStatus.Publish;
        await _repository.SaveChangesAsync();
        var tournamentToReturn = _mapper.Map<TournamentDTO>(tournament);
        return tournamentToReturn;
    }
}