using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserService(FantasyFootballContext db) : IUserService
{
    public async Task<AppUser> GetUser(string userId)
    {
        var user = await db.Users.FindAsync(userId);
        if (user == null) throw new Exception("Could not find user");
        return user;
    }

    public async Task<IList<AppUser>> GetUsers()
    {
        return await db.Users.ToListAsync();
    }
}