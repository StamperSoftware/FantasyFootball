using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AthleteService(FantasyFootballContext db) : IAthleteService
{
    
    public async Task<IList<Athlete>> GetAthletesWithTeams() => await db.Athletes.Include(a => a.Team).ToListAsync();
    
}