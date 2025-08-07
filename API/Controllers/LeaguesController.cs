using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LeaguesController(IGenericRepository<League> repo, ILeagueService service) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<League?>>> GetLeagues([FromQuery] LeagueSpecParams specParams)
    {
        var spec = new LeagueSpecification(specParams);
        return await CreatePagedResult(repo, spec, specParams.PageIndex, specParams.PageSize);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LeagueDto>> GetLeague(int id)
    {
        var league = await service.GetLeagueWithFullDetailsAsync(id);
        
        if (league is null) return BadRequest($"could not find league with id:{id}");
        return Ok(new LeagueDto(league));
    }
    
    [HttpPost]
    public async Task<ActionResult<League>> CreateLeague(CreateLeagueDto leagueDto)
    {
        if (string.IsNullOrWhiteSpace(leagueDto.Name)) return BadRequest("Must have a name");
        var league = new League()
        {
            Name = leagueDto.Name,
            Season = 2025,
        };

        var settings = new LeagueSettings(league.Id);
        league.Settings = settings;
        
        repo.Add(league);
        
        if (await repo.SaveAllAsync()) return CreatedAtAction("GetLeague", new { id = league.Id }, league);
        
        return BadRequest("Issue creating league");
    }

    [HttpPost("{leagueId:int}/players/{playerId:int}")]
    public async Task<ActionResult> AddPlayerToLeague(int playerId, int leagueId)
    {
        try
        {
            await service.AddPlayerToLeagueAsync(playerId, leagueId);
        }
        catch (Exception ex)
        {
            return BadRequest($"Could not add player, {ex.Message}");
        }

        return Ok();
    }
    
    
    [HttpPut("{leagueId:int}/team/{teamId:int}/athletes/{athleteId:int}")]
    public async Task AddAthleteToTeamAsync([FromRoute]int leagueId, [FromRoute]int teamId, [FromRoute]int athleteId)
    {
        await service.AddAthleteToTeamAsync(leagueId, teamId, athleteId);
    }

    [HttpPost("{leagueId:int}/draft")]
    public async Task SubmitDraft(int leagueId, SubmitDraftRequest request)
    {
        Dictionary<int, IList<int>> teamAthleteDictionary = [];
        
        foreach (var team in request.Results)
        {
            teamAthleteDictionary.Add(team.TeamId, team.Athletes);
        }
        
        await service.SubmitDraft(leagueId, teamAthleteDictionary);
    }

    [HttpPost("{leagueId:int}/schedule")]
    public async Task CreateSchedule(int leagueId)
    {
        await service.CreateSchedule(leagueId);
    }


    [HttpGet("{leagueId:int}/available-athletes")]
    public async Task<ActionResult<IList<Athlete>>> GetAvailableAthletes(int leagueId)
    {
        return Ok(await service.GetAvailableAthletes(leagueId));
    }


} 