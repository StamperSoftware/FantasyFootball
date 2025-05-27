using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;
public class LeagueService(FantasyFootballContext db, IGenericRepository<Player> playerRepo, IGenericRepository<UserTeam> userTeamRepo, IUserTeamService userTeamService, IScheduleService scheduleService) : ILeagueService
{
    const int MAX_TEAMS_IN_LEAGUE = 10;//todo set with league setting
    
    public async Task AddPlayerToLeagueAsync(int playerId, int leagueId)
    {
        var player = await playerRepo.GetByIdAsync(playerId);
        if (player == null) throw new Exception("Could not find player");

        var league = await GetLeagueWithFullDetailsAsync(leagueId);
        if (league == null) throw new Exception("Could not find league");
        
        if (league.Teams.Count >= MAX_TEAMS_IN_LEAGUE) throw new Exception("League is full.");
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
        await scheduleService.CreateSchedule(league);
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
                .ThenInclude(s => s.Games)
            .FirstOrDefaultAsync(l => l.Id == id);
    }
    
    
    private static bool IsPlayerAvailable(League league, IList<int> athleteIds)
    {
        return !league.Teams.Any(t => t.Athletes.Any(a => athleteIds.Contains(a.Id)));
    }
    
}

