using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Interfaces;
using Core.Validators;
using Infrastructure.Data.MongoDb;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Services;

public class RosterService:IRosterService
{

    private readonly IMongoCollection<Roster> _rosters;
    private readonly ILeagueSettingsService _leagueSettingsService;
    
    public RosterService(IOptions<DbSettings> dbSettings, ILeagueSettingsService leagueSettingsService)
    {
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
        _rosters = mongoDb.GetCollection<Roster>(dbSettings.Value.Rosters);
        _leagueSettingsService = leagueSettingsService;
    }
    
    public async Task<Roster> GetRoster(string id)
    {
        return await _rosters.Find(r => r.Id == id).FirstOrDefaultAsync() ?? throw new Exception("Could not get roster");
    }

    public async Task DeleteRoster(string id)
    {
        await _rosters.DeleteOneAsync(id);
    }
    
    public async Task DeleteRosters(IEnumerable<string> rosterIds)
    {
        await _rosters.DeleteManyAsync(r => rosterIds.Contains(r.Id));
    }
    
    private async Task<ValidationResult> UpdateRoster(Roster roster)
    {
        
        var leagueSettings = await _leagueSettingsService.GetLeagueSettings(roster.LeagueId) ?? throw new Exception("Could not get league settings");
        var validation = roster.Validate(leagueSettings);
        if (validation == ValidationResult.Success)
        {
            roster.Starters = roster.Starters.OrderBy(a => a.Position).ToList();
            roster.Bench = roster.Bench.OrderBy(a => a.Position).ToList();
            await _rosters.ReplaceOneAsync(r => r.Id == roster.Id, roster);
        }

        return validation;
    }

    public async Task<Roster> CreateRoster(int leagueId)
    {
        var roster = new Roster
        {
            Starters = [],
            Bench = [],
            LeagueId = leagueId
        };
        await _rosters.InsertOneAsync(roster);
        return roster;
    }

    public async Task<ValidationResult> AddAthlete(Athlete athlete, string rosterId)
    {
        var roster = await GetRoster(rosterId) ?? throw new Exception("Could not get roster");
        roster.Bench.Add(athlete);
        return await UpdateRoster(roster);
    }
    
    public async Task<ValidationResult> DropAthlete(Athlete athlete, string rosterId)
    {
        var roster = await GetRoster(rosterId) ?? throw new Exception("Could not get roster");
        roster.Starters = roster.Starters.Where(a => a.Id != athlete.Id).ToList();
        roster.Bench = roster.Bench.Where(a => a.Id != athlete.Id).ToList();
        return await UpdateRoster(roster);
    }
    
    public async Task<ValidationResult> BenchAthlete(Athlete athlete, string rosterId)
    {
        var roster = await GetRoster(rosterId) ?? throw new Exception("Could not get roster");
        if (roster.Starters.All(a => a.Id != athlete.Id)) throw new Exception("Athlete is not on the starter");
        roster.Starters = roster.Starters.Where(a => a.Id != athlete.Id).ToList();
        roster.Bench.Add(athlete);
        return await UpdateRoster(roster);
    }
    
    public async Task<ValidationResult> StartAthlete(Athlete athlete, string rosterId)
    {
        var roster = await GetRoster(rosterId) ?? throw new Exception("Could not get roster");
        if (roster.Bench.All(a => a.Id != athlete.Id)) throw new Exception("Athlete is not on the bench");
        roster.Bench = roster.Bench.Where(a => a.Id != athlete.Id).ToList();
        roster.Starters.Add(athlete);
        return await UpdateRoster(roster);
    }

    public async Task HandleTradeAsync(UserTeam teamOne, UserTeam teamTwo, IList<Athlete> teamOneAthletes, IList<Athlete> teamTwoAthletes)
    {
        foreach (var athlete in teamOneAthletes)
        {
            if (teamOne.Roster.Starters.Any(a => a.Id == athlete.Id))
            {
                teamOne.Roster.Starters = teamOne.Roster.Starters.Where(a => a.Id != athlete.Id).ToList();
            }
            else
            {
                teamOne.Roster.Bench = teamOne.Roster.Bench.Where(a => a.Id != athlete.Id).ToList();
            }

            teamTwo.Roster.Bench.Add(athlete);
        }
        
        foreach (var athlete in teamTwoAthletes)
        {
            if (teamTwo.Roster.Starters.Any(a => a.Id == athlete.Id))
            {
                teamTwo.Roster.Starters = teamTwo.Roster.Starters.Where(a => a.Id != athlete.Id).ToList();
            }
            else
            {
                teamTwo.Roster.Bench = teamTwo.Roster.Bench.Where(a => a.Id != athlete.Id).ToList();
            }

            teamOne.Roster.Bench.Add(athlete);
        }

        await UpdateRoster(teamOne.Roster);
        await UpdateRoster(teamTwo.Roster);
    }

    
}
