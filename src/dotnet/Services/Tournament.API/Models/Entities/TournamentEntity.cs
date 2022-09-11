using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Domains;
using Contracts.Domains.Implementations;

namespace Tournament.API.Models.Entities;

internal class TournamentEntity : EntityBase<Guid>
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }
    
    public DateTimeOffset CreateTime { get; set; }

    public DateTimeOffset EndTime { get; set; }

    public GameCategory GameCategory { get; set; }

    public CompetitionFormat CompetitionFormat { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    [MaxLength(500)]
    public string CancelReason { get; set; }

    // public TournamentStatus Status { get; set; }
}