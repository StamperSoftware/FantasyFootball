using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserTeamService(FantasyFootballContext db) : IUserTeamService
{
    public async Task<UserTeam?> GetUserTeamFullDetail(int id)
    {
        return await db.UserTeams
            .Include(t => t.Player)
                .ThenInclude(p => p.User)
            .Include(t => t.Athletes)
                .ThenInclude(a => a.Team)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddAthleteToTeam(int teamId, int athleteId)
    {
        var team = await db.UserTeams.FindAsync(teamId);
        if (team == null) throw new Exception("Could not get team");

        var athlete = await db.Athletes.FindAsync(athleteId);
        if (athlete == null) throw new Exception("Could not get athlete");

        team.Athletes.Add(athlete);

        await db.SaveChangesAsync();
    }
}