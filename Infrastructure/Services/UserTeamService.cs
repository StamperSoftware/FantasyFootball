using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.MongoDb;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Services;

public class UserTeamService : IUserTeamService
{
    private IMongoCollection<TradeRequest> TradeRequests { get; set; }
    private FantasyFootballContext Db { get; set; }
    private IRosterService RosterService { get; set; }
    private IGameService GameService { get; set; }
    private IAthleteService AthleteService { get; set; }
    
    public UserTeamService(FantasyFootballContext db, IRosterService rosterService, IGameService gameService, IOptions<DbSettings> dbSettings, IAthleteService athleteService)
    {
        Db = db;
        RosterService = rosterService;
        GameService = gameService;
        AthleteService = athleteService;
        
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
        TradeRequests = mongoDb.GetCollection<TradeRequest>(dbSettings.Value.TradeRequests);
    }

    private async Task<ValidationResult> _AddAthleteToTeamAsync(UserTeam team, int athleteId)
    {
        var athlete = await AthleteService.GetAthlete(athleteId) ?? throw new Exception("Could not get athlete");
        return await RosterService.AddAthlete(athlete, team.RosterId);
    }

    private async Task<UserTeam> _GetTeamAsync(int teamId)
    {
        var team = await Db.UserTeams.FindAsync(teamId) ?? throw new Exception("Could not get team");
        team.Roster = await RosterService.GetRoster(team.RosterId);
        return team;
    }

    public async Task<IList<UserTeam>> GetTeams()
    {
        return await Db.UserTeams.ToListAsync();
    }
    
    public async Task<IList<UserTeam>> GetTeams(string userId)
    {
        return await Db.UserTeams.Include(t => t.Player).ThenInclude(p => p.User).Where(t => t.Player.UserId == userId).ToListAsync();
    }
    
    public async Task<UserTeam?> GetUserTeamFullDetailAsync(int id)
    {
        var team = await Db.UserTeams
            .Include(t => t.Player)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (team is null) return null;
        
        team.Roster = await RosterService.GetRoster(team.RosterId);
        return team;
    }
    public async Task<UserTeam?> GetUserTeamScheduleAsync(int id)
    {
        var team = await Db.UserTeams
            .Include(t => t.Player)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (team is null) return null;
        
        team.Roster = await RosterService.GetRoster(team.RosterId);
        team.Schedule = await GameService.GetUserGames(id);
        return team;
    }

    public async Task<ValidationResult> AddAthleteToTeamAsync(int teamId, int athleteId)
    {
        var team = await _GetTeamAsync(teamId);
        return await _AddAthleteToTeamAsync(team, athleteId);
    }
    
    public async Task AddAthletesToTeamAsync(int teamId, IList<int> athleteIds)
    {
        var team = await _GetTeamAsync(teamId);
        foreach (var athleteId in athleteIds)
        {
            await _AddAthleteToTeamAsync(team, athleteId);
        }

        await Db.SaveChangesAsync();
    }

    public async Task AddAthletesToTeamsAsync(IDictionary<int, IList<int>> teamAthletesDictionary)
    {
        foreach (var (teamId, athleteIds) in teamAthletesDictionary)
        {
            var team = await _GetTeamAsync(teamId);
            foreach (var athleteId in athleteIds)
            {
                await _AddAthleteToTeamAsync(team, athleteId);
            }
        }
    }
    
    public async Task TradeAthletesAsync(int teamOneId, int teamTwoId, IList<int> teamOneAthleteIds,
        IList<int> teamTwoAthleteIds)
    {
        var teamOne = await GetUserTeamFullDetailAsync(teamOneId);
        var teamTwo = await GetUserTeamFullDetailAsync(teamTwoId);

        if (teamOne == null || teamTwo == null) throw new Exception("Could not find team");
        var athletes = await AthleteService.GetAthletesWithTeamsAsync();
        await RosterService.HandleTradeAsync(teamOne, teamTwo, athletes.Where(a => teamOneAthleteIds.Contains(a.Id)).ToList(), athletes.Where(a => teamTwoAthleteIds.Contains(a.Id)).ToList());
    }
    
    public async Task DropAthleteFromTeamAsync(int teamId, int athleteId)
    {
        var athlete = await AthleteService.GetAthlete(athleteId);
        if (athlete == null) throw new Exception("Could not get athlete");

        var team = await GetUserTeamFullDetailAsync(teamId);
        if (team == null) throw new Exception("Could not get team");

        await RosterService.DropAthlete(athlete, team.RosterId);
    }
    
    public async Task<ValidationResult> MoveAthleteToBench(int teamId, int athleteId)
    {
        var athlete = await AthleteService.GetAthlete(athleteId);
        if (athlete == null) throw new Exception("Could not get athlete");

        var team = await GetUserTeamFullDetailAsync(teamId);
        if (team == null) throw new Exception("Could not get team");

        return await RosterService.BenchAthlete(athlete, team.RosterId);
    }
    
    public async Task<ValidationResult> MoveAthleteToStarters(int teamId, int athleteId)
    {
        var athlete = await AthleteService.GetAthlete(athleteId);
        if (athlete == null) throw new Exception("Could not get athlete");

        var team = await GetUserTeamFullDetailAsync(teamId);
        if (team == null) throw new Exception("Could not get team");

       return await RosterService.StartAthlete(athlete, team.RosterId);
    }

    public async Task<UserTeam> CreateUserTeam(int leagueId, Player player)
    {
        var userTeam = UserTeam.CreateNewTeam(leagueId, player.Id,$"Team {player.User.UserName}");
        var roster = await RosterService.CreateRoster(leagueId);
        userTeam.RosterId = roster.Id;
        await Db.AddAsync(userTeam);
        if (await Db.SaveChangesAsync() == 0) throw new Exception("Could not create team.");
        return userTeam;
    }

    public async Task CreateTradeRequestAsync(int initiatingTeamId, int receivingTeamId, IList<int> initiatingAthleteIds, IList<int> receivingAthleteIds)
    {

        var initiatingTeam = await GetUserTeamFullDetailAsync(initiatingTeamId) ?? throw new Exception("Could not get team");
        var receivingTeam = await GetUserTeamFullDetailAsync(receivingTeamId) ?? throw new Exception("Could not get team");

        if (initiatingTeam.LeagueId != receivingTeam.LeagueId) throw new Exception("Teams are not in the same league");

        if (initiatingAthleteIds.Any(id =>
                !initiatingTeam.Roster.Starters.Union(initiatingTeam.Roster.Bench).Select(t => t.Id).Contains(id)))
            throw new Exception("Athlete is not on initiating team");
        
        if (receivingAthleteIds.Any(id =>
                !receivingTeam.Roster.Starters.Union(receivingTeam.Roster.Bench).Select(t => t.Id).Contains(id)))
            throw new Exception("Athlete is not on receiving team");
        var tradeRequest = TradeRequest.Create(initiatingTeam.LeagueId, initiatingTeam, receivingTeam, initiatingAthleteIds, receivingAthleteIds);
        
        await TradeRequests.InsertOneAsync(tradeRequest);
    }

    public async Task<IList<TradeRequest>> GetReceivedTradeRequests(int teamId)
    {
        return await TradeRequests.Find(t => t.ReceivingTeamId == teamId).ToListAsync() ?? throw new Exception("Could not get request");
    }
    public async Task<IList<TradeRequest>> GetInitiatedTradeRequests(int teamId)
    {
        return await TradeRequests.Find(t => t.InitiatingTeamId == teamId).ToListAsync() ?? throw new Exception("Could not get request");
    }

    public async Task ConfirmTradeRequest(string requestId)
    {
        var request = await TradeRequests.Find(tr => tr.Id == requestId).FirstOrDefaultAsync() ?? throw new Exception("Could not get request");
        await TradeAthletesAsync(request.InitiatingTeamId, request.ReceivingTeamId, request.InitiatingAthleteIds, request.ReceivingAthleteIds);
        await TradeRequests.DeleteOneAsync(tr => tr.Id == requestId);
    }
    public async Task DeclineTradeRequest(string requestId)
    {
        await TradeRequests.DeleteOneAsync(tr => tr.Id == requestId);
    }
}