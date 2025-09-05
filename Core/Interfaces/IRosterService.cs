using Core.Entities;

namespace Core.Interfaces;

public interface IRosterService
{
    Task<Roster> GetRoster(string id);
    Task UpdateRoster(Roster roster);
    Task<Roster> CreateRoster();
    Task AddAthlete(Athlete athlete, string rosterId);
    Task DropAthlete(Athlete athlete, string rosterId);
    Task MoveAthleteToBench(Athlete athlete, string rosterId);
    Task MoveAthleteToStarters(Athlete athlete, string rosterId);
    Task DeleteRoster(string rosterId);
    Task DeleteRosters(IEnumerable<string> rosterIds);

    Task HandleTradeAsync(UserTeam teamOne, UserTeam teamTwo, IList<Athlete> teamOneAthletes,
        IList<Athlete> teamTwoAthletes);
}