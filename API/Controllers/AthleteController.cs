using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AthleteController(IGenericRepository<Athlete> repo, IAthleteService service, FantasyFootballContext db, ISiteSettingsService siteSettingsService):BaseApiController
{
    private Random rand = new();
    
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AthleteDto>>> GetAthletes()
    {
        var athletes = await service.GetAthletesWithTeamsAsync();
        
        return Ok(athletes.Select(athlete => new AthleteDto(athlete)));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AthleteDto>> GetAthlete(int id)
    {
        return Ok(await repo.GetByIdAsync(id));
    }

    [HttpGet("{athleteId:int}/with-stats")]
    public async Task<ActionResult> GetAthleteWithStats(int athleteId)
    {
        return Ok(await service.GetAthleteWithStatsAsync(athleteId));
    }
    

    [HttpPut("{athleteId:int}/stats")]
    public async Task<ActionResult> UpdateAthleteStats(int athleteId, UpdateAthleteStatsRequest request)
    {
        try
        {
            await service.UpdateAthleteWeeklyStatsAsync(athleteId, request.Week, request.Season, request.Receptions,
                request.ReceivingYards, request.ReceivingTouchdowns, request.PassingYards, request.PassingTouchdowns,
                request.RushingYards, request.RushingTouchdowns);
        }
        catch
        {
            return BadRequest("Could not save athlete stats");
        }

        return Ok();
    }

    [HttpPost("generate-weekly-stats")]
    public async Task<ActionResult> GenerateWeeklyStats()
    {
        var siteSettings = await siteSettingsService.GetSettings();
        var athletes = await repo.ListAllAsync();
        List<AthleteWeeklyStats> aws = [];
        
        foreach (var athlete in athletes)
        {
            if (athlete == null) continue;
            
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
                    passingYards = rand.Next(150,450);
                    passingTouchdowns = rand.Next(0, 5);
                    rushingYards = rand.Next(0,50);
                    rushingTouchdowns = rand.Next(0, 2);

                    break;
                case Position.RunningBack:
                    receptions = rand.Next(0, 5);
                    receivingYards =  receptions > 0 ? rand.Next(10,30) : 0;
                    receivingTouchdowns = receptions > 0 ? rand.Next(0, receptions/2) : 0;
                    rushingYards = rand.Next(40,200);
                    rushingTouchdowns = rand.Next(0, 5);
                    break;
                case Position.WideReceiver:
                    receptions = rand.Next(3, 10);
                    receivingYards = rand.Next(60,160);
                    receivingTouchdowns = rand.Next(0, (receptions/2)+1);
                    break;
                case Position.TightEnd:
                    receptions = rand.Next(3, 7);
                    receivingYards =  rand.Next(30,90);
                    receivingTouchdowns = rand.Next(0, receptions/2);
                    break;
                case Position.Defense:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            aws.Add(new AthleteWeeklyStats(siteSettings.CurrentWeek, siteSettings.CurrentSeason, athlete.Id)
            {
                Receptions=receptions,
                ReceivingYards = receivingYards, 
                ReceivingTouchdowns = receivingTouchdowns,
                PassingYards = passingYards, 
                PassingTouchdowns = passingTouchdowns, 
                RushingYards = rushingYards, 
                RushingTouchdowns = rushingTouchdowns
            });
        }

        await db.AthleteWeeklyStats.AddRangeAsync(aws);
        await db.SaveChangesAsync();
        return Ok();
    }
}