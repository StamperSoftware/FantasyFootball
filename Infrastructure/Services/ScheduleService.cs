using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services;

public class ScheduleService(IGenericRepository<Schedule> repository) : IScheduleService
{

    private Random random = new();
    
    public async Task CreateSchedule(League league)
    {
        Schedule schedule = new(league.Id);

        for (var week = 1; week <= league.NumberOfGames; week++)
        {
            HashSet<int> playedTeamIds = [];
            
            foreach (var team in league.Teams)
            {
                if (playedTeamIds.Count == league.Teams.Count) break;
                if (playedTeamIds.Contains(team.Id)) continue;
                
                var matchup = league.Teams
                    .Where(t => t.Id != team.Id)
                    .Where(t => !playedTeamIds.Contains(t.Id))
                    .OrderBy(t => random.Next())
                    .First();
                
                schedule.Games.Add(new Game(team, matchup, week));
                playedTeamIds.Add(team.Id);
                playedTeamIds.Add(matchup.Id);
            }
        }

        repository.Add(schedule);
        await repository.SaveAllAsync();
    }
}