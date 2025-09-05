using Core.Entities;

namespace Core.Interfaces;

public interface IGameService
{
    public Task<Game?> GetFullDetailAsync(string gameId);
    public Task<Game?> GetGame(string gameId);
    public Task<List<Game>> GetUserGames(int userId);
    public Task<List<Game>> GetLeagueGames(int leagueId);
    public Task AddGames(IList<Game> games);
    public Task FinalizeGameAsync(string gameId);
    public Task FinalizeGamesAsync();
    public Task UpdateScoreAsync(string gameId);
    public Task DeleteGames(IEnumerable<string> gameIds);
}