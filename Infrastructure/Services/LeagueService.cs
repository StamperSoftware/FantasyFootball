﻿using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;
public class LeagueService(FantasyFootballContext db, IGenericRepository<Player> playerRepo, IGenericRepository<UserTeam> userTeamRepo, IUserTeamService userTeamService) : ILeagueService
{
    const int MAX_TEAMS_IN_LEAGUE = 10;
    public async Task AddPlayerToLeagueAsync(int playerId, int leagueId)
    {
        var player = await playerRepo.GetByIdAsync(playerId);
        if (player == null) throw new Exception("Could not find player");

        var league = await GetLeagueWithFullDetailsAsync(leagueId);
        if (league == null) throw new Exception("Could not find league");
        
        if (league.Teams.Count >= MAX_TEAMS_IN_LEAGUE) throw new Exception("League is full.");
        if (league.Teams.Any(t => t.PlayerId == playerId)) throw new Exception("Player is in league already.");

        var userTeam = new UserTeam { LeagueId = leagueId, PlayerId = playerId, Player = player, Name = $"(NEW) Team #{league.Teams.Count + 1}"};
        userTeamRepo.Add(userTeam);
        if (!await userTeamRepo.SaveAllAsync()) throw new Exception("Could not create team."); 
        
        league.Teams.Add(userTeam);
        player.Teams.Add(userTeam);
        await db.SaveChangesAsync();
    }

    public async Task SubmitDraft(IDictionary<int, IList<int>> request)
    {
        await userTeamService.AddAthletesToTeamsAsync(request);
    }

    public async Task<League?> GetLeagueWithFullDetailsAsync(int id)
    {
        return await db.Leagues
            .Include(l => l.Teams.OrderBy(t => id))
                .ThenInclude(t => t.Player)
                    .ThenInclude(p => p.User)
                .Include(l => l.Teams)
                    .ThenInclude(t => t.Athletes.OrderBy(a => a.Position))
                        .ThenInclude(a => a.Team)
            .FirstOrDefaultAsync(l => l.Id == id);
    }
}

