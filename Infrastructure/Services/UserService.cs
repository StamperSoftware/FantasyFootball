using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserService(FantasyFootballContext db) : IUserService
{
    public async Task<AppUser> GetUser(string userId)
    {
        return await db.Users.FindAsync(userId) ?? throw new Exception("Could not find user");
    }

    public async Task<IList<AppUser>> GetUsers()
    {
        return await db.Users.Where(u => u.EmailConfirmed).ToListAsync();
    }
}