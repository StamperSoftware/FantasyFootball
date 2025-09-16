using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Core.Interfaces;

public interface IRosterService
{
    Task<Roster> GetRoster(string id);
    Task<Roster> CreateRoster(int leagueId);
    Task<ValidationResult> AddAthlete(Athlete athlete, string rosterId);
    Task<ValidationResult> DropAthlete(Athlete athlete, string rosterId);
    Task<ValidationResult> BenchAthlete(Athlete athlete, string rosterId);
    Task<ValidationResult> StartAthlete(Athlete athlete, string rosterId);
    Task DeleteRoster(string rosterId);
    Task DeleteRosters(IEnumerable<string> rosterIds);
    Task HandleTradeAsync(UserTeam teamOne, UserTeam teamTwo, IList<Athlete> teamOneAthletes, IList<Athlete> teamTwoAthletes);
}