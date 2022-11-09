namespace Tournament.API.Controllers.Tournaments;

public class TournamentQueryParameters : QueryParameters
{
    public DateTime? BeginDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}