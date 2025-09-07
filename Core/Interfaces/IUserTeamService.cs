using Core.Entities;

namespace Core.Interfaces;

public interface IUserTeamService
{
    public Task<UserTeam?> GetUserTeamFullDetailAsync(int id);
    public Task<IList<UserTeam>> GetTeams();
    public Task<IList<UserTeam>> GetTeams(string userId);
    public Task<UserTeam?> GetUserTeamScheduleAsync(int id);
    public Task AddAthleteToTeamAsync(int teamId, int athleteId);
    public Task AddAthletesToTeamAsync(int teamId, IList<int> athleteId);
    public Task AddAthletesToTeamsAsync(IDictionary<int, IList<int>> teamAthleteDictionary);
    public Task TradeAthletesAsync(int teamOneId, int teamTwoId, IList<int> teamOneAthleteIds, IList<int> teamTwoAthleteIds);
    public Task DropAthleteFromTeamAsync(int teamId, int athleteId);
    public Task MoveAthleteToBench(int teamId, int athleteId);
    public Task MoveAthleteToStarters(int teamId, int athleteId);
    public Task<UserTeam> CreateUserTeam(int leagueId, Player player, int teamCount);
}