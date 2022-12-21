using Tournament.API.Models.Entities;

namespace Tournament.API.Extensions;

public static class TournamentExtensions
{
    public static int GetStatusIndex(this TournamentEntity tournament)
    {
        var now = DateTimeOffset.UtcNow;
        if (now > tournament.EndTime)
        {
            return -1;
        }

        if (now >= tournament.BeginTime && now <= tournament.EndTime)
        {
            return 0;
        }

        return 1;
    }
}