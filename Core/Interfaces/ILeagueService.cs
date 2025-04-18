using Core.Entities;

namespace Core.Interfaces;

public interface ILeagueService
{
    public Task AddPlayerToLeagueAsync(int playerId, int leagueId);
    public Task<League?> GetLeagueWithFullDetailsAsync(int id);
}