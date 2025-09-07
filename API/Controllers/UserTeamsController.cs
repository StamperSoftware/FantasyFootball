using API.DTOs;
using API.Extensions;
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
        return Ok(await userTeamService.GetTeams());
    } 
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IReadOnlyList<UserTeamDto>>> GetUserTeams(string userId)
    {
        return Ok(await userTeamService.GetTeams(userId));
    }

    [HttpGet("{teamId:int}")]
    public async Task<ActionResult<UserTeamDto>> GetTeam(int teamId)
    {
        var team = await userTeamService.GetUserTeamFullDetailAsync(teamId);
        if (team == null) return BadRequest("Could not find team");
        return Ok(team.Convert());
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
    
    [HttpPut("trade-athletes")]
    public async Task TradeAthletes(TradeAthleteRequest request)
    {
        await userTeamService.TradeAthletesAsync(request.TeamOneId, request.TeamTwoId, request.TeamOneAthleteIds, request.TeamTwoAthleteIds);
    }

    [HttpDelete("{teamId:int}/athletes/{athleteId:int}")]
    public async Task DropAthleteFromTeam(int teamId, int athleteId)
    {
        await userTeamService.DropAthleteFromTeamAsync(teamId, athleteId);
    }
    [HttpPut("{teamId:int}/bench/{athleteId:int}")]
    public async Task MoveAthleteToBench(int teamId, int athleteId)
    {
        await userTeamService.MoveAthleteToBench(teamId, athleteId);
    }
    [HttpPut("{teamId:int}/starters/{athleteId:int}")]
    public async Task MoveAthleteToStarters(int teamId, int athleteId)
    {
        await userTeamService.MoveAthleteToStarters(teamId, athleteId);
    }
    
    
}