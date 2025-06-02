using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AthleteService(FantasyFootballContext db) : IAthleteService
{
    
    public async Task<IList<Athlete>> GetAthletesWithTeamsAsync() => await db.Athletes.Include(a => a.Team).ToListAsync();

    public async Task UpdateAthleteWeeklyStatsAsync(int athleteId, int week, int season, int receptions, int receivingYards,
        int receivingTouchdowns, int passingYards, int passingTouchdowns, int rushingYards, int rushingTouchdowns)
    {
        var athlete = await db.Athletes.FindAsync(athleteId);
        if (athlete == null) throw new Exception("Could not find athlete");

        var stats = await db.AthleteWeeklyStats.Where(stat => stat.Week == week).Where(stat => stat.AthleteId == athleteId).Where(stat => stat.Season == season).FirstOrDefaultAsync();

        if (stats != null)
        {
            stats.PassingTouchdowns = passingTouchdowns;
            stats.PassingYards = passingYards;
            stats.RushingTouchdowns = rushingTouchdowns;
            stats.RushingYards = rushingYards;
            stats.ReceivingTouchdowns = receivingTouchdowns;
            stats.ReceivingYards = receivingYards;
            stats.Receptions = receptions;
            
            db.AthleteWeeklyStats.Update(stats);
        }
        else
        {
            stats = new AthleteWeeklyStats(athlete, week, season, receptions, receivingYards, receivingTouchdowns, passingYards, passingTouchdowns, rushingYards, rushingTouchdowns);
            await db.AthleteWeeklyStats.AddAsync(stats);
        }
        
        await db.SaveChangesAsync();
    }
}