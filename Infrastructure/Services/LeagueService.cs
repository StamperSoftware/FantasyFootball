using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Factories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;
public class LeagueService(FantasyFootballContext db, IUserService userService, IUserTeamService userTeamService, ISiteSettingsService siteSettingService, ILeagueSettingsService leagueSettingsService, IRosterService rosterService, IGameService gameService) : ILeagueService
{
    
    private readonly Random _random = new();
    private readonly SiteSettings _siteSettings = siteSettingService.GetSettings().Result;
    public async Task AddUserToLeagueAsync(string userId, int leagueId)
    {
        var user = await userService.GetUser(userId) ?? throw new Exception("Could not find user");
        var league = await GetLeagueWithFullDetailsAsync(leagueId) ?? throw new Exception("Could not find league");
        
        if (league.Teams.Count >= league.Settings.NumberOfTeams) throw new Exception("League is full.");
        if (league.Teams.Any(t => t.UserId == userId)) throw new Exception("User is in league already.");
        
        var userTeam = await userTeamService.CreateUserTeam(leagueId, user);
        
        league.Teams.Add(userTeam);
        league.Settings.DraftOrder.Add(userTeam.Id);
        
        await leagueSettingsService.UpdateLeagueSettings(league.Settings);
        await db.SaveChangesAsync();
    }

    public async Task<ValidationResult> AddAthleteToTeamAsync(int leagueId, int teamId, int athleteId)
    {
        var league = await GetLeagueWithFullDetailsAsync(leagueId) ?? throw new Exception("Could not get league");
        if (!IsPlayerAvailable(league, [athleteId])) throw new Exception("Athlete is already on a team");
        return await userTeamService.AddAthleteToTeamAsync(teamId, athleteId);
    }

    public async Task SubmitDraft(int leagueId, IDictionary<int, IList<int>> request)
    {
        var league = await GetLeagueWithFullDetailsAsync(leagueId) ?? throw new Exception("Could not get league");
        await userTeamService.AddAthletesToTeamsAsync(request);
    }

    public async Task CreateSchedule(int leagueId)
    {
        var league = await GetLeagueWithFullDetailsAsync(leagueId) ?? throw new Exception("Could not get league");
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
                    .OrderBy(t => _random.Next())
                    .First();
                
                league.Schedule.Add(GameFactory.CreateGame(team, matchup, week, _siteSettings.CurrentSeason, leagueId));
                playedTeamIds.Add(team.Id);
                playedTeamIds.Add(matchup.Id);
            }
        }

        await gameService.AddGames(league.Schedule);
    }

    public async Task DeleteLeague(int leagueId)
    {
        var league = await db.Leagues.Include(l => l.Teams).FirstOrDefaultAsync(l => l.Id == leagueId) ?? throw new Exception("Could not get league");
        
        db.Leagues.Remove(league);
        await rosterService.DeleteRosters(league.Teams.Select(t => t.RosterId));
        await leagueSettingsService.DeleteLeagueSettings(leagueId);
        await gameService.DeleteGames(league.Schedule.Select(s => s.Id));
        await db.SaveChangesAsync();
    }
    
    public async Task<League?> GetLeagueWithFullDetailsAsync(int id)
    {
        var league =  await db.Leagues
            .Include(l => l.Teams.OrderBy(t => id))
            .FirstOrDefaultAsync(l => l.Id == id) ?? throw new Exception("Could not get league");

        league.Settings = await leagueSettingsService.GetLeagueSettings(league.Id) ?? throw new Exception("Could not get settings");
        league.Schedule = await gameService.GetLeagueGames(league.Id) ?? throw new Exception("Could not get games");
        
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

    public async Task<bool> IsUserInLeague(string userId, int leagueId)
    {
        var league = await db.Leagues
            .Include(l => l.Teams)
            .FirstOrDefaultAsync(l => l.Id == leagueId) ?? throw new Exception("Could not get league");
        
        return league.Teams.Any(t => t.UserId == userId);
    }

    public async Task<IList<AppUser>> GetUsersNotInLeague(int leagueId)
    {
        var league = await db.Leagues.Include(l => l.Teams).FirstOrDefaultAsync(l => l.Id == leagueId) ?? throw new Exception("Could not get league");
        var users = await userService.GetConfirmedAppUsers();
        return users.Where(p => league.Teams.All(t => t.UserId != p.Id)).ToList();
    }

    public async Task<League> CreateLeague(string name, string adminId)
    {
        var siteSettings = await siteSettingService.GetSettings();
        var league = League.CreateLeague(name, siteSettings.CurrentSeason, adminId);
        await db.Leagues.AddAsync(league);
        await db.SaveChangesAsync();
        league.Settings = await leagueSettingsService.CreateLeagueSettings(league.Id);
        return league;
    }
    
}

