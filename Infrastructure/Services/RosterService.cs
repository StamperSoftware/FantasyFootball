using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data.MongoDb;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Services;

public class RosterService:IRosterService
{

    private readonly IMongoCollection<Roster> _rosters;

    public RosterService(IOptions<DbSettings> dbSettings)
    {
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
        _rosters = mongoDb.GetCollection<Roster>(dbSettings.Value.Rosters);
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
    
    public async Task UpdateRoster(Roster roster)
    {
        roster.Starters = roster.Starters.OrderBy(a => a.Position).ToList();
        roster.Bench = roster.Bench.OrderBy(a => a.Position).ToList();
        await _rosters.ReplaceOneAsync(r => r.Id == roster.Id, roster);
    }

    public async Task<Roster> CreateRoster()
    {
        var roster = new Roster
        {
            Starters = [],
            Bench = [],
        };
        await _rosters.InsertOneAsync(roster);
        return roster;
    }

    public async Task AddAthlete(Athlete athlete, string rosterId)
    {
        var roster = await GetRoster(rosterId) ?? throw new Exception("Could not get roster");
        roster.Bench.Add(athlete);
        await UpdateRoster(roster);
    }
    
    public async Task DropAthlete(Athlete athlete, string rosterId)
    {
        var roster = await GetRoster(rosterId) ?? throw new Exception("Could not get roster");
        roster.Starters = roster.Starters.Where(a => a.Id != athlete.Id).ToList();
        roster.Bench = roster.Bench.Where(a => a.Id != athlete.Id).ToList();
        await UpdateRoster(roster);
    }
    
    public async Task MoveAthleteToBench(Athlete athlete, string rosterId)
    {
        var roster = await GetRoster(rosterId) ?? throw new Exception("Could not get roster");
        if (roster.Starters.All(a => a.Id != athlete.Id)) throw new Exception("Athlete is not on the starter");
        roster.Starters = roster.Starters.Where(a => a.Id != athlete.Id).ToList();
        roster.Bench.Add(athlete);
        await UpdateRoster(roster);
    }
    
    public async Task MoveAthleteToStarters(Athlete athlete, string rosterId)
    {
        var roster = await GetRoster(rosterId) ?? throw new Exception("Could not get roster");
        if (roster.Bench.All(a => a.Id != athlete.Id)) throw new Exception("Athlete is not on the bench");
        roster.Bench = roster.Bench.Where(a => a.Id != athlete.Id).ToList();
        roster.Starters.Add(athlete);
        await UpdateRoster(roster);
    }

    public async Task HandleTradeAsync(UserTeam teamOne, UserTeam teamTwo, IList<Athlete> teamOneAthletes, IList<Athlete> teamTwoAthletes)
    {
        foreach (var athlete in teamOneAthletes)
        {
            teamOne.Roster.Bench = teamOne.Roster.Bench.Where(a => a.Id != athlete.Id).ToList();
            teamOne.Roster.Starters = teamOne.Roster.Starters.Where(a => a.Id != athlete.Id).ToList();
            teamTwo.Roster.Bench.Add(athlete);
        }
        
        foreach (var athlete in teamTwoAthletes)
        {
            
            teamTwo.Roster.Bench = teamTwo.Roster.Bench.Where(a => a.Id != athlete.Id).ToList();
            teamTwo.Roster.Starters = teamTwo.Roster.Starters.Where(a => a.Id != athlete.Id).ToList();
            teamOne.Roster.Bench.Add(athlete);
        }

        await UpdateRoster(teamOne.Roster);
        await UpdateRoster(teamTwo.Roster);
    }
}
