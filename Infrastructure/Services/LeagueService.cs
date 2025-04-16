﻿using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;
public class LeagueService(FantasyFootballContext db, IGenericRepository<Player> playerRepo, IGenericRepository<UserTeam> userTeamRepo) : ILeagueService
{
    const int MAX_TEAMS_IN_LEAGUE = 3;
    public async Task AddPlayerToLeagueAsync(int playerId, int leagueId)
    {
        var player = await playerRepo.GetByIdAsync(playerId);
        if (player == null) throw new Exception("Could not find player");

        var league = await GetLeagueWithTeamsAsync(leagueId);
        if (league == null) throw new Exception("Could not find league");
        
        if (league.Teams.Count >= MAX_TEAMS_IN_LEAGUE) throw new Exception("League is full.");
        if (league.Teams.Any(t => t.PlayerId == playerId)) throw new Exception("Player is in league already.");

        var userTeam = new UserTeam { LeagueId = leagueId, PlayerId = playerId, Player = player};
        userTeamRepo.Add(userTeam);
        if (!await userTeamRepo.SaveAllAsync()) throw new Exception("Could not create team."); 
        
        league.Teams.Add(userTeam);
        player.Teams.Add(userTeam);
        await db.SaveChangesAsync();
    }

    public async Task<League?> GetLeagueWithTeamsAsync(int id)
    {
        return await db.Leagues
            .Include(l => l.Teams)
                .ThenInclude(l => l.Player)
            .FirstOrDefaultAsync(l => l.Id == id);
    }
}