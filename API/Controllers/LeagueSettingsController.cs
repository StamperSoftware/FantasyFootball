using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/league-settings")]
public class LeagueSettingsController(FantasyFootballContext db):BaseApiController
{
    [HttpGet("{leagueId:int}")]
    public async Task<LeagueSettings> GetLeagueSettings(int leagueId)
    {
        return await db.LeagueSettings.FirstOrDefaultAsync(ls => ls.LeagueId == leagueId) ?? throw new Exception("Could not get league settings");
    }

    [HttpPut("{leagueId:int}")]
    public async Task UpdateLeagueSettings(LeagueSettings updateLeagueSettings)
    {
        db.Update(updateLeagueSettings);
        await db.SaveChangesAsync();
    }
}