using Core.Entities;

namespace Core.Interfaces;

public interface IGameService
{
    public Task<Game?> GetFullDetailAsync(int gameId);
    public Task<Game?> GetGame(int gameId);
    public Task FinalizeGameAsync(int gameId);
    public Task FinalizeGamesAsync();
    public Task UpdateScoreAsync(int gameId);
}