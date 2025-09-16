using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Core.Interfaces;

public interface IUserTeamService
{
    public Task<UserTeam?> GetUserTeamFullDetailAsync(int id);
    public Task<IList<UserTeam>> GetTeams();
    public Task<IList<UserTeam>> GetTeams(string userId);
    public Task<UserTeam?> GetUserTeamScheduleAsync(int id);
    public Task<ValidationResult> AddAthleteToTeamAsync(int teamId, int athleteId);
    public Task AddAthletesToTeamAsync(int teamId, IList<int> athleteId);
    public Task AddAthletesToTeamsAsync(IDictionary<int, IList<int>> teamAthleteDictionary);
    public Task TradeAthletesAsync(int teamOneId, int teamTwoId, IList<int> teamOneAthleteIds, IList<int> teamTwoAthleteIds);
    public Task DropAthleteFromTeamAsync(int teamId, int athleteId);
    public Task<ValidationResult> MoveAthleteToBench(int teamId, int athleteId);
    public Task<ValidationResult> MoveAthleteToStarters(int teamId, int athleteId);
    public Task<UserTeam> CreateUserTeam(int leagueId, Player player);
    public Task CreateTradeRequestAsync(int initiatingTeamId, int receivingTeamId, IList<int> initiatingAthleteIds, IList<int> receivingAthleteIds);
    public Task<IList<TradeRequest>> GetReceivedTradeRequests(int teamId);
    public Task<IList<TradeRequest>> GetInitiatedTradeRequests(int teamId);
    public Task ConfirmTradeRequest(string requestId);
    public Task DeclineTradeRequest(string requestId);
}