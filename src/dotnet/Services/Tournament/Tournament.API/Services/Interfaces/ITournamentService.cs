using Tournament.API.Controllers.Payloads;
using Tournament.API.Controllers.Tournaments;
using Tournament.API.Models.DTOs;
using Tournament.API.Services.Implementations;

namespace Tournament.API.Services.Interfaces;

public interface ITournamentService
{
    Task<Page<TournamentDTO>> GetTournaments(TournamentQueryParameters parameters);
    Task<TournamentDTO?> GetTournamentById(Guid id);
    Task<TournamentDTO> CreateNewTournament(DraftTournamentRequest tournament);
    Task<TournamentDTO> UpdateTournament(Guid id, UpdateTournamentRequest tournament);
    Task<TournamentDTO> CancelTournament(Guid id);
    Task<TournamentDTO> DeleteTournament(Guid id);
    Task<TournamentDTO> ValidateTournamentToPublish(Guid tournamentId);
    Task<TournamentDTO> PublishTournament(Guid tournamentId);
}