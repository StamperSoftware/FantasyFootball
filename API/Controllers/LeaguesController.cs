using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LeaguesController(IGenericRepository<League> repo) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<League?>>> GetLeagues([FromQuery] LeagueSpecParams specParams)
    {
        var spec = new LeagueSpecification(specParams);
        return await CreatePagedResult(repo, spec, specParams.PageIndex, specParams.PageSize);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<League>> GetLeague(int id)
    {
        var league = await repo.GetByIdAsync(id);
        if (league is null) return BadRequest($"could not find league with id:{id}");
        return Ok(league);
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
}