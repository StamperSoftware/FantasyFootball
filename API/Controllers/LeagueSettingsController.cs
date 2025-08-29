using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/league-settings")]
public class LeagueSettingsController(ILeagueSettingsService leagueSettingsService):BaseApiController
{
    [HttpGet("{leagueId:int}")]
    public async Task<LeagueSettings> GetLeagueSettings(int leagueId)
    {
        return await leagueSettingsService.GetLeagueSettings(leagueId) ?? throw new Exception("Could not get league settings");
    }

    [HttpPut("{leagueId:int}")]
    public async Task UpdateLeagueSettings(LeagueSettings updateLeagueSettings)
    {
        await leagueSettingsService.UpdateLeagueSettings(updateLeagueSettings);
    }

    [HttpPost("{leagueId:int}")]
    public async Task CreateLeagueSettings(int leagueId)
    {
        await leagueSettingsService.CreateLeagueSettings(leagueId);
    }
}