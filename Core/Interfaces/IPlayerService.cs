using Core.Entities;

namespace Core.Interfaces;

public interface IPlayerService
{
    public Task<Player> GetPlayer(int playerId);
    public Task<IList<Player>> GetPlayers();
}