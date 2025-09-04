using Core.Entities;

namespace Core.Interfaces;

public interface ILeagueSettingsService
{
    public Task<LeagueSettings?> GetLeagueSettings(int leagueId);

    Task UpdateLeagueSettings(LeagueSettings updateLeagueSettings);
 
    Task<LeagueSettings> CreateLeagueSettings(int leagueId);
    Task DeleteLeagueSettings(int leagueId);
}