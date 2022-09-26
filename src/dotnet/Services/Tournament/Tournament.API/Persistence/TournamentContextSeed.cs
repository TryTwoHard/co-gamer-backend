using Tournament.API.Models.Entities;
using ILogger = Serilog.ILogger;
namespace Tournament.API.Persistence;

public class TournamentContextSeed
{
    public static async Task SeedTournamentAsync(TournamentContext tournamentContext, ILogger logger)
    {
        if (!tournamentContext.Tournaments.Any())
        {
            tournamentContext.AddRange(getTournaments());
            await tournamentContext.SaveChangesAsync();
            logger.Information("Seeded data for Product DB associated with context {DbContextName}",
                nameof(TournamentContext));
        }
    }

    private static IEnumerable<TournamentEntity> getTournaments()
    {
        return new List<TournamentEntity>
        {
            new()
            {
                Id = Guid.Parse("6a52b6aa-bc87-4b7d-899b-c2f143e0ebec"),
                Name = "Valorant 1",
                HostId = Guid.Parse("b7bc6647-6df7-4a61-9c72-4f432f5b4e27"),
                Description = "A tournament for valoranter",
                BeginTime = new DateTime(2022, 9, 27),
                EndTime = new DateTime(2022, 10, 5),
                CreateTime = DateTimeOffset.UtcNow,
                CancelReason = null,
                CompetitionFormat = new CompetitionFormat(),
                GameCategory = null
            },
            new()
            {
                Id = Guid.Parse("878c60ba-d187-4744-8e62-a0c49dc8f89a"),
                Name = "Valorant 2",
                HostId = Guid.Parse("f817eae5-f016-42e4-9dd7-bab43ad526e6"),
                Description = "A tournament for valoranter 2",
                BeginTime = new DateTime(2022, 10, 27),
                EndTime = new DateTime(2022, 11, 5),
                CreateTime = DateTimeOffset.UtcNow,
                CancelReason = null,
                CompetitionFormat = new CompetitionFormat(),
                GameCategory = null
            }
        };
    }
}