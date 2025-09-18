using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Core.Interfaces;

public interface ILeagueService
{
    public Task AddPlayerToLeagueAsync(int playerId, int leagueId);
    public Task<League?> GetLeagueWithFullDetailsAsync(int id);
    public Task<ValidationResult> AddAthleteToTeamAsync(int leagueId, int teamId, int athleteId);
    public Task<League> CreateLeague(string name, int adminId);
    public Task SubmitDraft(int leagueId, IDictionary<int, IList<int>> teamAthleteDictionary);
    public Task CreateSchedule(int leagueId);
    public Task<IList<Athlete>> GetAvailableAthletes(int leagueId);
    public Task DeleteLeague(int leagueId);
    public Task<bool> IsUserInLeague(string userId, int leagueId);
    public Task<IList<Player>> GetPlayersNotInLeague(int leagueId);
}
