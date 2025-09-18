using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class PlayerService(FantasyFootballContext db):IPlayerService
{
    public async Task<Player> GetPlayer(int playerId)
    {
        return await db.Players.Include(p => p.User).FirstOrDefaultAsync(p=> p.Id == playerId) ?? throw new Exception("Could not get player");
    }

    public async Task<Player> GetPlayerByUserId(string userId)
    {
        return await db.Players.Include(p => p.User).FirstOrDefaultAsync(p => p.UserId == userId) ?? throw new Exception("Could not get player");
    }

    public async Task<IList<Player>> GetPlayers()
    {
        return await db.Players.Include(p => p.User).Where(p => p.User.EmailConfirmed).ToListAsync();
    }

    public async Task<Player> CreatePlayer(string firstName, string lastName, AppUser user)
    {
        var player = Player.CreateNewPlayer(firstName, lastName, user);
        await db.Players.AddAsync(player);
        await db.SaveChangesAsync();
        return player;
    }
}