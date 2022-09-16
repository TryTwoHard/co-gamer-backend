using System.ComponentModel.DataAnnotations;
using Contracts.Domains.Implementations;
using Tournament.API.Models.Statuses;

namespace Tournament.API.Models.Entities.Tournament;

public class TournamentEntity : EntityBase<Guid>
{
    [MaxLength(100)]
    public string Name { get; set; }
    
    public DateTimeOffset CreateTime { get; set; }
    public DateTimeOffset BeginTime { get; set; }

    public DateTimeOffset EndTime { get; set; }

    public GameCategory GameCategory { get; set; }

    public CompetitionFormat CompetitionFormat { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    [MaxLength(500)]
    public string CancelReason { get; set; }

    public TournamentStatus Status { get; set; }
}