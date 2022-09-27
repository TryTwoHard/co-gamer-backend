using AutoMapper;
using Tournament.API.Controllers.Payloads;
using Tournament.API.Exceptions;
using Tournament.API.Models.DTOs;
using Tournament.API.Models.Entities;
using Tournament.API.Models.Statuses;
using Tournament.API.Repositories.Interfaces;
using Tournament.API.Services.Interfaces;
using Tournament.API.Specifications;

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

    public async Task<Page<TournamentDTO>> GetTournaments(int? pageIndex, int? pageSize, int? status, Guid? hostId, Guid? gameId)
    {
        var response = new Page<TournamentDTO>(pageSize);
        pageIndex ??= 0;
        pageSize ??= 0;
        status ??= 0;
        // status: -1 - Past,  0 - Current, 1 - Future, 2 - All, default: 0
        var pagedSpec = new TournamentFilterPaginatedSpecification(
            skip: pageIndex.Value * pageSize.Value,
            take: pageSize.Value,
            status: status,
            hostId: hostId,
            gameId: gameId
        );
        
        var list = await _repository.GetTournamentsAsync(pagedSpec);
        response.Content.AddRange(_mapper.Map<IEnumerable<TournamentDTO>>(list));

        return response;
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
        if (tournament!.BeginTime <= now)
        {
            throw new TournamentInvalidException($"Tournament's already been published!");
        }

        if (tournament!.EndTime <= now)
        {
            throw new TournamentInvalidException("Tournament's ready ended!");
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