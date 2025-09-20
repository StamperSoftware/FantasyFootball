using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
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
        var teams = await userTeamService.GetTeams(userId);
        return Ok(teams.Select(t => t.Convert()));
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
        var team = await userTeamService.GetUserTeamFullDetailAsync(request.Id);
        var contextUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (team == null) return BadRequest($"Could not get team with id {request.Id}");
        if (team.UserId != contextUserId) return BadRequest("Not Users team");
        
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
    [HttpPost("trade-request")]
    public async Task CreateTradeRequest(TradeAthleteRequest request)
    {
        await userTeamService.CreateTradeRequestAsync(request.TeamOneId, request.TeamTwoId, request.TeamOneAthleteIds, request.TeamTwoAthleteIds);
    }

    [HttpDelete("{teamId:int}/athletes/{athleteId:int}")]
    public async Task DropAthleteFromTeam(int teamId, int athleteId)
    {
        await userTeamService.DropAthleteFromTeamAsync(teamId, athleteId);
    }
    [HttpPut("{teamId:int}/bench/{athleteId:int}")]
    public async Task<IActionResult> MoveAthleteToBench(int teamId, int athleteId)
    {
        var result = await userTeamService.MoveAthleteToBench(teamId, athleteId);
        if (result != ValidationResult.Success) return BadRequest(result.ErrorMessage);
        return Ok();
    }
    [HttpPut("{teamId:int}/starters/{athleteId:int}")]
    public async Task<IActionResult> MoveAthleteToStarters(int teamId, int athleteId)
    {
        var result = await userTeamService.MoveAthleteToStarters(teamId, athleteId);
        if (result != ValidationResult.Success) return BadRequest(result.ErrorMessage);
        return Ok();
    }

    [HttpGet("{teamId:int}/received-trade-requests")]
    public async Task<IList<TradeRequestTeamDto>> GetReceivingTradeRequests(int teamId)
    {
        var requests = await userTeamService.GetReceivedTradeRequests(teamId);
        return requests.Select(tr => tr.ConvertReceived()).ToList();
    }
    
    [HttpGet("{teamId:int}/initiated-trade-requests")]
    public async Task<IList<TradeRequestTeamDto>> GetInitiatedTradeRequests(int teamId)
    {
        var requests = await userTeamService.GetInitiatedTradeRequests(teamId);

        return requests.Select(tr => tr.ConvertInitiated()).ToList();
    }
    
    [HttpPut("trade-requests/{requestId}/confirm")]
    public async Task ConfirmTradeRequest(string requestId)
    {
        await userTeamService.ConfirmTradeRequest(requestId);
    }
    [HttpPut("trade-requests/{requestId}/decline")]
    public async Task DeclineTradeRequest(string requestId)
    {
        await userTeamService.DeclineTradeRequest(requestId);
    }
    
}