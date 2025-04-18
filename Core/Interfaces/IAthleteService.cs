using Core.Entities;

namespace Core.Interfaces;

public interface IAthleteService
{
    public Task<IList<Athlete>> GetAthletesWithTeams();
}