using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AthleteService(FantasyFootballContext db, ISiteSettingsService siteSettingsService) : IAthleteService
{
    private readonly Random _random = new();
    
    public async Task<IList<Athlete>> GetAthletesWithTeamsAsync() => await db.Athletes.Include(a => a.Team).ToListAsync();
    public async Task<IList<Athlete>> GetAthletes() => await db.Athletes.ToListAsync();

    public async Task<Athlete> GetAthlete(int athleteId)
    {
        return await db.Athletes.Include(a => a.Team).FirstOrDefaultAsync(a => a.Id == athleteId) ?? throw new Exception("Could not get athlete");
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
            stats = AthleteWeeklyStats.Create(week, season, athlete.Id, receptions, receivingYards, receivingTouchdowns,
                passingYards, passingTouchdowns, rushingYards, rushingTouchdowns);
                
            await db.AthleteWeeklyStats.AddAsync(stats);
        }
        
        await db.SaveChangesAsync();
    }

    public async Task GenerateWeeklyStats()
    {
        var siteSettings = await siteSettingsService.GetSettings();
        var athletes = await db.Athletes.ToListAsync();
        List<AthleteWeeklyStats> aws = [];
        
        foreach (var athlete in athletes)
        {
            
            var receptions = 0;
            var receivingYards =  0;
            var receivingTouchdowns = 0;
            var passingYards = 0;
            var passingTouchdowns = 0;
            var rushingYards = 0;
            var rushingTouchdowns = 0;

            switch (athlete.Position)
            {
                case Position.QuarterBack:
                    passingYards = _random.Next(150,450);
                    passingTouchdowns = _random.Next(0, 5);
                    rushingYards = _random.Next(0,50);
                    rushingTouchdowns = _random.Next(0, 2);

                    break;
                case Position.RunningBack:
                    receptions = _random.Next(0, 5);
                    receivingYards =  receptions > 0 ? _random.Next(10,30) : 0;
                    receivingTouchdowns = receptions > 0 ? _random.Next(0, receptions/2) : 0;
                    rushingYards = _random.Next(40,200);
                    rushingTouchdowns = _random.Next(0, 5);
                    break;
                case Position.WideReceiver:
                    receptions = _random.Next(3, 10);
                    receivingYards = _random.Next(60,160);
                    receivingTouchdowns = _random.Next(0, (receptions/2)+1);
                    break;
                case Position.TightEnd:
                    receptions = _random.Next(3, 7);
                    receivingYards =  _random.Next(30,90);
                    receivingTouchdowns = _random.Next(0, receptions/2);
                    break;
                case Position.Defense:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var newStats = AthleteWeeklyStats.Create(siteSettings.CurrentWeek, siteSettings.CurrentSeason, athlete.Id,
                receptions, receivingYards, receivingTouchdowns, passingYards, passingTouchdowns, rushingYards,
                rushingTouchdowns);

            aws.Add(newStats);
        }

        await db.AthleteWeeklyStats.AddRangeAsync(aws);
        await db.SaveChangesAsync();
    }
}