namespace Tournament.API.Controllers.Tournaments;

public class TournamentQueryParameters : QueryParameters
{
    public string State { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}