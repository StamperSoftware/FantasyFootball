using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserService(FantasyFootballContext db, UserManager<AppUser> userManager) : IUserService
{
    public async Task<AppUser> GetUser(string userId)
    {
        return await db.Users.FindAsync(userId) ?? throw new Exception("Could not find user");
    }

    public async Task<IList<AppUser>> GetConfirmedAppUsers()
    {
        var adminUsers = await userManager.GetUsersInRoleAsync("SiteAdmin");
        var users = await userManager.Users.Where(u => u.EmailConfirmed).ToListAsync();
        return users.Except(adminUsers).ToList();
    }
}