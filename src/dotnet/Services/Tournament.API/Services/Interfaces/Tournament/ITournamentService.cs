using System.Linq.Expressions;
using Tournament.API.Models.DTOs;
using Tournament.API.Models.Entities.Tournament;

namespace Tournament.API.Services.Interfaces.Tournament;

public interface ITournamentService
{
    Task<IEnumerable<TournamentDTO>> GetTournaments();
    Task<TournamentDTO?> GetTournamentById(Guid id);
    Task CreateNewTournament(TournamentDTO tournament);
    Task UpdateTournament(TournamentDTO tournament);
    Task CancelTournament(Guid id);
    Task DeleteTournament(Guid id);
    Task<bool> ValidateTournamentToPublish(Guid tournamentId);
    Task PublishTournament(Guid tournamentId);
}