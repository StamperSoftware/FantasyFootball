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

        var league = await db.Leagues.Include(l => l.Teams)
            .ThenInclude(t => t.Athletes)
            .FirstOrDefaultAsync(l => l.Id == team.LeagueId);
            
        if (league == null) throw new Exception("Could not get league");

        if (league.Teams.Any(t => t.Athletes.Any(a => a.Id == athleteId)))
            throw new Exception("Athlete is already on team");
        
        var athlete = await db.Athletes.FindAsync(athleteId);
        if (athlete == null) throw new Exception("Could not get athlete");
        
        team.Athletes.Add(athlete);

        await db.SaveChangesAsync();
    }

    public async Task TradeAthletes(int teamOneId, int teamTwoId, IList<int> teamOneAthleteIds,
        IList<int> teamTwoAthleteIds)
    {
        var teamOne = await GetUserTeamFullDetail(teamOneId);
        var teamTwo = await GetUserTeamFullDetail(teamTwoId);

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
}