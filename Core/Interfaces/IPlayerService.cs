using Core.Entities;

namespace Core.Interfaces;

public interface IPlayerService
{
    public Task<Player> GetPlayer(int playerId);
    public Task<Player> GetPlayerByUserId(string userId);
    public Task<IList<Player>> GetPlayers();
    public Task<Player> CreatePlayer(string firstName, string lastName, AppUser user);
}