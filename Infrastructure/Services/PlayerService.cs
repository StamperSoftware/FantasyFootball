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

    public async Task<IList<Player>> GetPlayers()
    {
        return await db.Players.Include(p => p.User).Where(p => p.User.EmailConfirmed).ToListAsync();
    }
}