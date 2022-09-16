using Tournament.API.Models.Entities.Tournament;
using Tournament.API.Models.Statuses;

namespace Tournament.API.Models.DTOs;

public class TournamentDTO
{
    public string Name { get; set; }
    
    public DateTimeOffset BeginTime { get; set; }

    public DateTimeOffset EndTime { get; set; }
    public DateTimeOffset CreateTime { get; set; }

    public GameCategory GameCategory { get; set; }

    public CompetitionFormat CompetitionFormat { get; set; }

    public string Description { get; set; }

    public string CancelReason { get; set; }
    public TournamentStatus Status { get; set; }
}