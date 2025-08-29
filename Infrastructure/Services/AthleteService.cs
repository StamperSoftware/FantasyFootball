using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AthleteService(FantasyFootballContext db) : IAthleteService
{
    
    public async Task<IList<Athlete>> GetAthletesWithTeamsAsync() => await db.Athletes.Include(a => a.Team).ToListAsync();
    public async Task<IList<Athlete>> GetAthletes() => await db.Athletes.ToListAsync();

    public async Task<Athlete> GetAthlete(int athleteId)
    {
        return await db.Athletes.FirstOrDefaultAsync(a => a.Id == athleteId) ?? throw new Exception("Could not get athlete");
    }

    public async Task<Athlete> GetAthleteWithStatsAsync(int athleteId)
    {
        var athlete = await db.Athletes
            .Include(a => a.WeeklyStats)
            .FirstAsync(a => a.Id == athleteId);
        
        return athlete;
    }

    public async Task UpdateAthleteWeeklyStatsAsync(int athleteId, int week, int season, int receptions, int receivingYards,
        int receivingTouchdowns, int passingYards, int passingTouchdowns, int rushingYards, int rushingTouchdowns)
    {
        var athlete = await db.Athletes.FindAsync(athleteId);
        if (athlete == null) throw new Exception("Could not find athlete");

        var stats = await db.AthleteWeeklyStats.FirstOrDefaultAsync(aws => aws.AthleteId == athleteId && aws.Season == season && aws.Week == week);

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
            stats = new AthleteWeeklyStats(week, season, athlete.Id)
            {
                Receptions = receptions,
                ReceivingTouchdowns = receivingTouchdowns,
                ReceivingYards = receivingYards,
                RushingTouchdowns = rushingTouchdowns,
                RushingYards = rushingYards,
                PassingTouchdowns = passingTouchdowns,
                PassingYards = passingYards

            };
                
            await db.AthleteWeeklyStats.AddAsync(stats);
        }
        
        await db.SaveChangesAsync();
    }
}