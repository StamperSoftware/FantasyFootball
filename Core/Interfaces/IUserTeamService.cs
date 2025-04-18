using Core.Entities;

namespace Core.Interfaces;

public interface IUserTeamService
{
    public Task<UserTeam?> GetUserTeamFullDetail(int id);
    public Task AddAthleteToTeam(int teamId, int athleteId);

    public Task TradeAthletes(int teamOneId, int teamTwoId, IList<int> teamOneAthleteIds, IList<int> teamTwoAthleteIds);
}