using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class FantasyFootballContext(DbContextOptions<FantasyFootballContext> options) : DbContext(options)
{
    public DbSet<League> Leagues { get; set; }
}