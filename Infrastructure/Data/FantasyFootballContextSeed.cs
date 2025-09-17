using System.Text.Json;
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

        await CheckAndAddTeams(db);
        await CheckAndAddAthletes(db);
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
        var adminEmail = Environment.GetEnvironmentVariable("SITE_ADMIN_EMAIL");
        var adminUserName = Environment.GetEnvironmentVariable("SITE_ADMIN_USERNAME");

        if (adminEmail == null || adminUserName == null) throw new Exception("Could not create site admin");
        
        var user = new AppUser(adminEmail, adminUserName);
        
        if (!await userManager.Users.AnyAsync(u => u.Email == user.Email))
        {
            var password = Environment.GetEnvironmentVariable("SITE_ADMIN_PASSWORD") ?? throw new Exception("Could not create site admin");
            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, "SiteAdmin");
            user.EmailConfirmed = true;
            await db.SaveChangesAsync();
        }
    }

    private static async Task CheckAndAddTeams(FantasyFootballContext db)
    {
        if (!db.Teams.Any())
        {
            var teamsJSON = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/teams.json");
            var teams = JsonSerializer.Deserialize<List<Team>>(teamsJSON);

            if (teams == null) return;

            await db.Teams.AddRangeAsync(teams);
            await db.SaveChangesAsync();
        }
    }
    private static async Task CheckAndAddAthletes(FantasyFootballContext db)
    {
        if (!db.Athletes.Any())
        {
            var athletesJSON = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/athletes.json");
            var athletes = JsonSerializer.Deserialize<List<Athlete>>(athletesJSON);

            if (athletes == null) return;

            await db.Athletes.AddRangeAsync(athletes);
            await db.SaveChangesAsync();
        }
    }
    
}