using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserTeamService(FantasyFootballContext db, IRosterService rosterService) : IUserTeamService
{

    private async Task _AddAthleteToTeamAsync(UserTeam team, int athleteId)
    {
        var athlete = await db.Athletes.FindAsync(athleteId);
        if (athlete == null) throw new Exception("Could not get athlete");
        await rosterService.AddAthlete(athlete, team.RosterId);
    }

    private async Task<UserTeam> _GetTeamAsync(int teamId)
    {
        var team = await db.UserTeams.FindAsync(teamId);
        if (team == null) throw new Exception("Could not get team");
        team.Roster = await rosterService.GetRoster(team.RosterId);
        return team;
    }

    public async Task<UserTeam?> GetUserTeamFullDetailAsync(int id)
    {
        var team = await db.UserTeams
            .Include(t => t.Player)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (team is null) return null;
        
        team.Roster = await rosterService.GetRoster(team.RosterId);
        return team;
    }

    public async Task AddAthleteToTeamAsync(int teamId, int athleteId)
    {
        var team = await _GetTeamAsync(teamId);
        await _AddAthleteToTeamAsync(team, athleteId);
        await db.SaveChangesAsync();
    }
    
    public async Task AddAthletesToTeamAsync(int teamId, IList<int> athleteIds)
    {
        var team = await _GetTeamAsync(teamId);
        foreach (var athleteId in athleteIds)
        {
            await _AddAthleteToTeamAsync(team, athleteId);
        }

        await db.SaveChangesAsync();
    }

    public async Task AddAthletesToTeamsAsync(IDictionary<int, IList<int>> teamAthletesDictionary)
    {
        foreach (var (teamId, athleteIds) in teamAthletesDictionary)
        {
            var team = await _GetTeamAsync(teamId);
            foreach (var athleteId in athleteIds)
            {
                await _AddAthleteToTeamAsync(team, athleteId);
            }
        }
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
            await rosterService.DropAthlete(athlete, teamOne.RosterId);
            await rosterService.AddAthlete(athlete, teamTwo.RosterId);
        }
        
        foreach (var id in teamTwoAthleteIds)
        {
            var athlete = await athletes.FirstOrDefaultAsync(a => a.Id == id);
            if (athlete == null) throw new Exception($"Could not find athlete with id {id}");
            await rosterService.AddAthlete(athlete, teamOne.RosterId);
            await rosterService.DropAthlete(athlete, teamTwo.RosterId);
        }
    }
    
    public async Task DropAthleteFromTeamAsync(int teamId, int athleteId)
    {
        var athlete = await db.Athletes.FindAsync(athleteId);
        if (athlete == null) throw new Exception("Could not get athlete");

        var team = await GetUserTeamFullDetailAsync(teamId);
        if (team == null) throw new Exception("Could not get team");

        await rosterService.DropAthlete(athlete, team.RosterId);
    }
    
    public async Task MoveAthleteToBench(int teamId, int athleteId)
    {
        var athlete = await db.Athletes.FindAsync(athleteId);
        if (athlete == null) throw new Exception("Could not get athlete");

        var team = await GetUserTeamFullDetailAsync(teamId);
        if (team == null) throw new Exception("Could not get team");

        await rosterService.MoveAthleteToBench(athlete, team.RosterId);
    }
    
    public async Task MoveAthleteToStarters(int teamId, int athleteId)
    {
        var athlete = await db.Athletes.FindAsync(athleteId);
        if (athlete == null) throw new Exception("Could not get athlete");

        var team = await GetUserTeamFullDetailAsync(teamId);
        if (team == null) throw new Exception("Could not get team");

        await rosterService.MoveAthleteToStarters(athlete, team.RosterId);
    }

    public async Task<UserTeam> CreateUserTeam(int leagueId, Player player, int teamCount)
    {
        var userTeam = new UserTeam(leagueId, player, 0, 0, $"(NEW) Team #{teamCount + 1}");
        var roster = await rosterService.CreateRoster();
        userTeam.RosterId = roster.Id;
        await db.AddAsync(userTeam);
        if (await db.SaveChangesAsync() == 0) throw new Exception("Could not create team.");
        return userTeam;
    }
    
}