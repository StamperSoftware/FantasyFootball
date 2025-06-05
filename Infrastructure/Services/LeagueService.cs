using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;
public class LeagueService(FantasyFootballContext db, IGenericRepository<Player> playerRepo, IGenericRepository<UserTeam> userTeamRepo, IUserTeamService userTeamService) : ILeagueService
{
    
    private Random random = new();
    public async Task AddPlayerToLeagueAsync(int playerId, int leagueId)
    {
        var player = await playerRepo.GetByIdAsync(playerId);
        if (player == null) throw new Exception("Could not find player");

        var league = await GetLeagueWithFullDetailsAsync(leagueId);
        if (league == null) throw new Exception("Could not find league");
        
        if (league.Teams.Count >= league.Settings.NumberOfTeams) throw new Exception("League is full.");
        if (league.Teams.Any(t => t.PlayerId == playerId)) throw new Exception("Player is in league already.");

        var userTeam = new UserTeam(leagueId, player, 0, 0, $"(NEW) Team #{league.Teams.Count + 1}");
        userTeamRepo.Add(userTeam);
        if (!await userTeamRepo.SaveAllAsync()) throw new Exception("Could not create team."); 
        
        league.Teams.Add(userTeam);
        player.Teams.Add(userTeam);
        await db.SaveChangesAsync();
    }

    public async Task AddAthleteToTeamAsync(int leagueId, int teamId, int athleteId)
    {
        var league = await GetLeagueWithFullDetailsAsync(leagueId);
        if (league is null) throw new Exception("Could not get league");
        if (!IsPlayerAvailable(league, [athleteId])) throw new Exception("Athlete is already on a team");
        await userTeamService.AddAthleteToTeamAsync(teamId, athleteId);
    }

    public async Task SubmitDraft(int leagueId, IDictionary<int, IList<int>> request)
    {
        var league = await GetLeagueWithFullDetailsAsync(leagueId);
        if (league is null) throw new Exception("Could not get league");
        await userTeamService.AddAthletesToTeamsAsync(request);
    }

    public async Task CreateSchedule(int leagueId)
    {
        var league = await GetLeagueWithFullDetailsAsync(leagueId);
        if (league is null) throw new Exception("Could not get league");
        if (league.Schedule.Count > 0) throw new Exception("Schedule already created");
        
        for (var week = 1; week <= league.Settings.NumberOfGames; week++)
        {
            HashSet<int> playedTeamIds = [];
            
            foreach (var team in league.Teams)
            {
                if (playedTeamIds.Count == league.Teams.Count) break;
                if (playedTeamIds.Contains(team.Id)) continue;
                
                var matchup = league.Teams
                    .Where(t => t.Id != team.Id)
                    .Where(t => !playedTeamIds.Contains(t.Id))
                    .OrderBy(t => random.Next())
                    .First();
                
                league.Schedule.Add(new Game(team, matchup, week, 2025));
                playedTeamIds.Add(team.Id);
                playedTeamIds.Add(matchup.Id);
            }
        }

        await db.SaveChangesAsync();
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
            .Include(l => l.Schedule)
            .Include(l => l.Settings)
            .FirstOrDefaultAsync(l => l.Id == id);
    }
    
    
    private static bool IsPlayerAvailable(League league, IList<int> athleteIds)
    {
        return !league.Teams.Any(t => t.Athletes.Any(a => athleteIds.Contains(a.Id)));
    }
    
}

