namespace TournamentParticipant.API.Models.Entities;

internal class Participant
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Team? Team { get; set; }
}