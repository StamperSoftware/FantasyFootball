using Core.Entities;

namespace Core.Interfaces;

public interface IGameService
{
    public Task<Game?> GetFullDetailAsync(int gameId);
}