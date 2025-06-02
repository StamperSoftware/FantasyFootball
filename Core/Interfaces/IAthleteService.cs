using Core.Entities;

namespace Core.Interfaces;

public interface IAthleteService
{
    public Task<IList<Athlete>> GetAthletesWithTeamsAsync();
    public Task UpdateAthleteWeeklyStatsAsync(int athleteId, int week, int season, int receptions, int receivingYards, int receivingTouchdowns, int passingYards, int passingTouchdowns, int rushingYards, int rushingTouchdowns);
}