using Core.Entities;

namespace Core.Interfaces;

public interface ILeagueService
{
    public Task AddPlayerToLeagueAsync(int playerId, int leagueId);
    public Task<League?> GetLeagueWithFullDetailsAsync(int id);
    public Task SubmitDraft(IDictionary<int, IList<int>> teamAthleteDictionary);
}
