using Tournament.API.Models.Entities;

namespace Tournament.API.Controllers.Payloads;

public class UpdateTournamentRequest
{
    public string Name { get; set; }

    public DateTimeOffset BeginTime { get; set; }
    
    public DateTimeOffset EndTime { get; set; }

    public GameCategory GameCategory { get; set; }

    public CompetitionFormat CompetitionFormat { get; set; }

    public string Description { get; set; }

    public string CancelReason { get; set; }
}