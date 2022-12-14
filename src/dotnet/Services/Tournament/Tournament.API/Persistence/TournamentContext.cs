using Microsoft.EntityFrameworkCore;
using Tournament.API.Models.Entities;

namespace Tournament.API.Persistence;

public class TournamentContext : DbContext
{
    public TournamentContext(DbContextOptions<TournamentContext> options) : base(options)
    {

    }

    public DbSet<TournamentEntity> Tournaments { get; set; }
}