using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class GameService(FantasyFootballContext db):IGameService
{
    public async Task<Game?> GetFullDetailAsync(int gameId)
    {
        return await db.Games
            .Include(g => g.Away)
                .ThenInclude(u => u.Athletes.OrderBy(at => at.Position).ThenBy(at => at.LastName))
                    .ThenInclude(a => a.Team)
            .Include(g => g.Home)
                .ThenInclude(u => u.Athletes.OrderBy(at => at.Position).ThenBy(at => at.LastName))
                    .ThenInclude(a => a.Team)
            .Include(g => g.Away)
                .ThenInclude(u => u.Player)
                    .ThenInclude(p => p.User)
            .Include(g => g.Home)
                .ThenInclude(u => u.Player)
                    .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(g => g.Id == gameId);
    }
}