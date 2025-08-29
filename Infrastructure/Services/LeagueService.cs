using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;
public class LeagueService(FantasyFootballContext db, IGenericRepository<Player> playerRepo, IUserTeamService userTeamService, ISiteSettingsService siteSettingService, ILeagueSettingsService leagueSettingsService) : ILeagueService
{
    
    private readonly Random _random = new();
    public async Task AddPlayerToLeagueAsync(int playerId, int leagueId)
    {
        var player = await playerRepo.GetByIdAsync(playerId);
        if (player == null) throw new Exception("Could not find player");

        var league = await GetLeagueWithFullDetailsAsync(leagueId);
        if (league == null) throw new Exception("Could not find league");
        
        if (league.Teams.Count >= league.Settings.NumberOfTeams) throw new Exception("League is full.");
        if (league.Teams.Any(t => t.PlayerId == playerId)) throw new Exception("Player is in league already.");
        var userTeam = await userTeamService.CreateUserTeam(leagueId, player, league.Teams.Count);
        league.Teams.Add(userTeam);
        league.Settings.DraftOrder.Add(userTeam.Id);
        await leagueSettingsService.UpdateLeagueSettings(league.Settings);
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
        var league = await GetLeagueWithFullDetailsAsync(leagueId) ?? throw new Exception("Could not get league");
        if (league.Schedule.Count > 0) throw new Exception("Schedule already created");
        var siteSettings = await siteSettingService.GetSettings();
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
                    .OrderBy(t => _random.Next())
                    .First();
                
                league.Schedule.Add(new Game(team, matchup, week, siteSettings.CurrentSeason, false));
                playedTeamIds.Add(team.Id);
                playedTeamIds.Add(matchup.Id);
            }
        }

        await db.SaveChangesAsync();
    }

    public async Task DeleteLeague(int leagueId)
    {
        var league = await db.Leagues.FindAsync(leagueId) ?? throw new Exception("Could not get league");
        
        db.Leagues.Remove(league);
        await db.SaveChangesAsync();
    }
    
    public async Task<League?> GetLeagueWithFullDetailsAsync(int id)
    {
        var league =  await db.Leagues
            .Include(l => l.Teams.OrderBy(t => id))
                .ThenInclude(t => t.Player)
                    .ThenInclude(p => p.User)
            .Include(l => l.Schedule)
            .FirstOrDefaultAsync(l => l.Id == id) ?? throw new Exception("Could not get league");

        league.Settings = await leagueSettingsService.GetLeagueSettings(league.Id) ?? throw new Exception("Could not get settings"); 
        
        for (int i = 0; i < league.Teams.Count; i++)
        {
            league.Teams[i] = await userTeamService.GetUserTeamFullDetailAsync(league.Teams[i].Id) ?? throw new Exception("Could not get team.");
        }
        return league;
    }

    public async Task<IList<Athlete>> GetAvailableAthletes(int leagueId)
    {
        var league = await GetLeagueWithFullDetailsAsync(leagueId) ?? throw new Exception("Could not get league");
        var takenAthletes = league.Teams.SelectMany(t => t.Roster is not null ? t.Roster.Bench.Union(t.Roster.Starters).Select(a => a.Id) : []);
        return await db.Athletes.Where(a => !takenAthletes.Contains(a.Id)).ToListAsync();
    }

    private static bool IsPlayerAvailable(League league, IList<int> athleteIds)
    {
        return !league.Teams.Any(t => t.Roster is not null && t.Roster.Starters.Union(t.Roster.Bench).Any(a => athleteIds.Contains(a.Id)));
    }
    
    
}

