using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class FantasyFootballContextSeed
{
    public static async Task SeedAsync(FantasyFootballContext db, UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        await CheckAndAddRole(roleManager, "1", "SiteAdmin", "SITE_ADMIN");
        await CheckAndAddRole(roleManager, "2", "LeagueAdmin", "LEAGUE_ADMIN");

        await CheckAndAddSiteAdminUser(db, userManager);
        
    }

    private static async Task CheckAndAddRole(RoleManager<IdentityRole> roleManager, string id, string name, string normalizedName)
    {
        if (await roleManager.RoleExistsAsync(name) == false)
        {
            await roleManager.CreateAsync(new IdentityRole{Id=id, Name=name, NormalizedName=normalizedName});
        }
    }

    private static async Task CheckAndAddSiteAdminUser(FantasyFootballContext db, UserManager<AppUser> userManager)
    {
        var user = new AppUser
        {
            Email = Environment.GetEnvironmentVariable("SITE_ADMIN_EMAIL"),
            UserName = Environment.GetEnvironmentVariable("SITE_ADMIN_USERNAME")
        };
        
        if (await userManager.Users.AnyAsync(u => u.Email == user.Email) == false)
        {
            var password = Environment.GetEnvironmentVariable("SITE_ADMIN_PASSWORD") ?? throw new Exception("Could not create site admin");
            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, "SiteAdmin");
            await db.SaveChangesAsync();
        }
        
    }
    
}