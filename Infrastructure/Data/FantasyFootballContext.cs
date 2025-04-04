using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class FantasyFootballContext(DbContextOptions<FantasyFootballContext> options)
    : IdentityDbContext<AppUser>(options)
{
    public DbSet<League> Leagues { get; set; }
    public new DbSet<AppUser> Users { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<UserTeam> UserTeams { get; set; }

}