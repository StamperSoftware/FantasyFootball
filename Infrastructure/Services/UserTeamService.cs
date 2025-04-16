using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserTeamService(FantasyFootballContext db) : IUserTeamService
{
    public async Task<UserTeam?> GetUserTeamWithPlayer(int id)
    {
        return await db.UserTeams
            .Include(t => t.Player)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
    
}