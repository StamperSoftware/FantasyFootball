using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/user-teams")]
public class UserTeamsController(IGenericRepository<UserTeam> repo, IUserTeamService userTeamService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserTeamDto>>> GetTeams()
    {
        return Ok(await repo.ListAllAsync());
    }

    [HttpGet("{teamId:int}")]
    public async Task<ActionResult<UserTeamDto>> GetTeam(int teamId)
    {
        var team = await userTeamService.GetUserTeamFullDetail(teamId);
        if (team == null) return BadRequest("Could not find team");
        return Ok(new UserTeamDto(team));
    }

    [HttpPut]
    public async Task<ActionResult> UpdateTeamName(UpdateTeamNameRequest request)
    {
        var team = await repo.GetByIdAsync(request.Id);
        if (team == null) return BadRequest($"Could not get team with id {request.Id}");
        team.Name = request.Name;
        repo.Update(team);
        if (await repo.SaveAllAsync())
        {
            return Ok();
        }
        return BadRequest("Could not update team name.");
    }

    [HttpPut("{teamId:int}/athletes/{athleteId:int}")]
    public async Task AddAthleteToTeam([FromRoute]int teamId, [FromRoute]int athleteId)
    {
        await userTeamService.AddAthleteToTeam(teamId, athleteId);
    }
    
    [HttpPut("trade-athletes")]
    public async Task TradeAthletes(TradeAthleteRequest request)
    {
        await userTeamService.TradeAthletes(request.TeamOneId, request.TeamTwoId, request.TeamOneAthleteIds, request.TeamTwoAthleteIds);
    }
}