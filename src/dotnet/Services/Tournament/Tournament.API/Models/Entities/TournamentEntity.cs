using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Domains.Implementations;
using Tournament.API.Models.Statuses;

namespace Tournament.API.Models.Entities;

public class TournamentEntity : EntityBase<Guid>
{
    [ForeignKey("host_id")]
    public Guid HostId { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }
    
    public DateTimeOffset CreateTime { get; set; }
    public DateTimeOffset BeginTime { get; set; }

    public DateTimeOffset EndTime { get; set; }

    public GameCategory? GameCategory { get; set; }

    public CompetitionFormat CompetitionFormat { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    [MaxLength(500)]
    public string? CancelReason { get; set; }
    public TournamentStatus Status { get; set; }
    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return Tuple.Create(Id, Name, CreateTime, BeginTime, EndTime, GameCategory, CompetitionFormat).GetHashCode();
    }
}