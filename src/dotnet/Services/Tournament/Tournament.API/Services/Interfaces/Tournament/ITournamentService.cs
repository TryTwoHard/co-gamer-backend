using System.Linq.Expressions;
using Tournament.API.Controllers.Payloads.Requests;
using Tournament.API.Models.DTOs;
using Tournament.API.Models.Entities.Tournament;

namespace Tournament.API.Services.Interfaces.Tournament;

public interface ITournamentService
{
    Task<IEnumerable<TournamentDTO>> GetTournaments();
    Task<TournamentDTO?> GetTournamentById(Guid id);
    Task<TournamentDTO> CreateNewTournament(DraftTournamentRequest tournament);
    Task<TournamentDTO> UpdateTournament(Guid id, UpdateTournamentRequest tournament);
    Task<TournamentDTO> CancelTournament(Guid id);
    Task<TournamentDTO> DeleteTournament(Guid id);
    Task<bool> ValidateTournamentToPublish(Guid tournamentId);
    Task<TournamentDTO> PublishTournament(Guid tournamentId);
}