﻿using API.DTOs;
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
    public async Task<ActionResult<League>> CreateLeague(League league)
    {
        repo.Add(league);
        if (await repo.SaveAllAsync())
        {
            return CreatedAtAction("GetLeague", new { id = league.Id }, league);
        }
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

    [HttpPost("draft")]
    public async Task SubmitDraft(SubmitDraftRequest request)
    {
        Dictionary<int, IList<int>> teamAthleteDictionary = [];
        
        foreach (var team in request.Results)
        {
            teamAthleteDictionary.Add(team.TeamId, team.Athletes);
        }
        
        await service.SubmitDraft(teamAthleteDictionary);
    }
}