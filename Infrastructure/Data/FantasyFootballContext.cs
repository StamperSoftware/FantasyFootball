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
    
    public DbSet<Team> Teams { get; set; }
    
    public DbSet<Athlete> Athletes { get; set; }
    
    public DbSet<Schedule> Schedules { get; set; }
    
    public DbSet<Game> Games { get; set; }
    
}