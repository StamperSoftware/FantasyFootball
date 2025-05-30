﻿using Core.Entities;

namespace Core.Interfaces;

public interface IUserTeamService
{
    public Task<UserTeam?> GetUserTeamFullDetailAsync(int id);
    public Task AddAthleteToTeamAsync(int teamId, int athleteId);
    public Task AddAthletesToTeamAsync(int teamId, IList<int> athleteId);
    public Task AddAthletesToTeamsAsync(IDictionary<int, IList<int>> teamAthleteDictionary);
    public Task TradeAthletesAsync(int teamOneId, int teamTwoId, IList<int> teamOneAthleteIds, IList<int> teamTwoAthleteIds);
    public Task DropAthleteFromTeamAsync(int teamId, int athleteId);
}