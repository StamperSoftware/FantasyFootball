using API.DTOs;
using API.Extensions;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AthleteController(IAthleteService service):BaseApiController
{
    
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AthleteDto>>> GetAthletes()
    {
        var athletes = await service.GetAthletesWithTeamsAsync();
        return Ok(athletes.Select(athlete => athlete.Convert()));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AthleteDto>> GetAthlete(int id)
    {
        return Ok(await service.GetAthlete(id));
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
            
            return Ok();
        }
        catch
        {
            return BadRequest("Could not save athlete stats");
        }
    }

    [HttpPost("generate-weekly-stats")]
    public async Task<ActionResult> GenerateWeeklyStats()
    {
        await service.GenerateWeeklyStats();
        return Ok();
    }
}