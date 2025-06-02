using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AthleteController(IGenericRepository<Athlete> repo, IAthleteService service):BaseApiController
{
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

    [HttpPut("{athleteId:int}/stats")]
    public async Task<ActionResult> UpdateAthleteStats(int athleteId, UpdateAthleteStatsRequest request)
    {
        try
        {
            await service.UpdateAthleteWeeklyStatsAsync(athleteId, request.Week, request.Season, request.Stats.Receptions,
                request.Stats.ReceivingYards, request.Stats.ReceivingTouchdowns, request.Stats.PassingYards, request.Stats.PassingTouchdowns,
                request.Stats.RushingYards, request.Stats.RushingTouchdowns);
        }
        catch
        {
            return BadRequest("Could not save athlete stats");
        }

        return Ok();
    }
}