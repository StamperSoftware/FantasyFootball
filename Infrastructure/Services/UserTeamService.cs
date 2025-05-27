using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserTeamService(FantasyFootballContext db) : IUserTeamService
{

    private async Task addAthleteToTeamAsync(UserTeam team, int athleteId)
    {
        
        var athlete = await db.Athletes.FindAsync(athleteId);
        if (athlete == null) throw new Exception("Could not get athlete");
        
        team.Athletes.Add(athlete);
    }

    private async Task<UserTeam> getTeamAsync(int teamId)
    {
        
        var team = await db.UserTeams.FindAsync(teamId);
        if (team == null) throw new Exception("Could not get team");
        return team;
    }


    public async Task<UserTeam?> GetUserTeamFullDetailAsync(int id)
    {
        return await db.UserTeams
            .Include(t => t.Player)
                .ThenInclude(p => p.User)
            .Include(t => t.Athletes.OrderBy(a => a.Position))
                .ThenInclude(a => a.Team)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddAthleteToTeamAsync(int teamId, int athleteId)
    {
        var team = await getTeamAsync(teamId);
        await addAthleteToTeamAsync(team, athleteId);
        await db.SaveChangesAsync();
    }
    
    public async Task AddAthletesToTeamAsync(int teamId, IList<int> athleteIds)
    {
        var team = await getTeamAsync(teamId);
        foreach (var athleteId in athleteIds)
        {
            await addAthleteToTeamAsync(team, athleteId);
        }

        await db.SaveChangesAsync();
    }

    public async Task AddAthletesToTeamsAsync(IDictionary<int, IList<int>> teamAthletesDictionary)
    {
        foreach (var (teamId, athleteIds) in teamAthletesDictionary)
        {
            var team = await getTeamAsync(teamId);
            foreach (var athleteId in athleteIds)
            {
                await addAthleteToTeamAsync(team, athleteId);
            }
        }

        await db.SaveChangesAsync();
    }
    
    public async Task TradeAthletesAsync(int teamOneId, int teamTwoId, IList<int> teamOneAthleteIds,
        IList<int> teamTwoAthleteIds)
    {
        var teamOne = await GetUserTeamFullDetailAsync(teamOneId);
        var teamTwo = await GetUserTeamFullDetailAsync(teamTwoId);

        if (teamOne == null || teamTwo == null) throw new Exception("Could not find team");
        var athletes = db.Athletes.Where(a => teamOneAthleteIds.Contains(a.Id) || teamTwoAthleteIds.Contains(a.Id));
        foreach (var id in teamOneAthleteIds)
        {
            var athlete = await athletes.FirstOrDefaultAsync(a => a.Id == id);
            if (athlete == null) throw new Exception($"Could not find athlete with id {id}");
            teamOne.Athletes.Remove(athlete);
            teamTwo.Athletes.Add(athlete);
        }
        
        foreach (var id in teamTwoAthleteIds)
        {
            var athlete = await athletes.FirstOrDefaultAsync(a => a.Id == id);
            if (athlete == null) throw new Exception($"Could not find athlete with id {id}");
            teamTwo.Athletes.Remove(athlete);
            teamOne.Athletes.Add(athlete);
        }

        await db.SaveChangesAsync();
    }

    
    public async Task DropAthleteFromTeamAsync(int teamId, int athleteId)
    {
        var athlete = await db.Athletes.FindAsync(athleteId);
        if (athlete == null) throw new Exception("Could not get athlete");

        var team = await GetUserTeamFullDetailAsync(teamId);
        if (team == null) throw new Exception("Could not get team");

        if (!team.Athletes.Contains(athlete)) throw new Exception("Athlete was not on team");
        team.Athletes.Remove(athlete);
        await db.SaveChangesAsync();
    }
    
}