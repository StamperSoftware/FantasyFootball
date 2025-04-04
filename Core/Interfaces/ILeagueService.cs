using Core.Entities;

namespace Core.Interfaces;

public interface ILeagueService
{
    public Task AddTeamToLeagueAsync(int playerId, int leagueId);
    public Task<League?> GetLeagueWithTeamsAsync(int id);
}