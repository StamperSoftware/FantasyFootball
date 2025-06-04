using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class GameService(FantasyFootballContext db):IGameService
{
    public async Task<Game?> GetFullDetailAsync(int gameId)
    {

        var game = await db.Games
            .Include(g => g.Away)
                .ThenInclude(u => u.Athletes.OrderBy(at => at.Position).ThenBy(at => at.LastName))
                    .ThenInclude(a => a.Team)
            .Include(g => g.Home)
                .ThenInclude(u => u.Athletes.OrderBy(at => at.Position).ThenBy(at => at.LastName))
                    .ThenInclude(a => a.Team)
            .Include(g => g.Away)
                .ThenInclude(u => u.Player)
                    .ThenInclude(p => p.User)
            .Include(g => g.Home)
                .ThenInclude(u => u.Player)
                    .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(g => g.Id == gameId);

        if (game == null) throw new Exception("Could not get game");

        List<int> athleteIds = [];
        
        athleteIds.AddRange(game.Home.Athletes.Select(a => a.Id));
        athleteIds.AddRange(game.Away.Athletes.Select(a => a.Id));

        game.WeeklyStats = await db.AthleteWeeklyStats.Where(aws => aws.Season == 2025).Where(aws => aws.Week == game.Week).Where(aws => athleteIds.Contains(aws.AthleteId)).ToListAsync();

        foreach (var athlete in game.Home.Athletes)
        {
            var weeklyStats = game.WeeklyStats.FirstOrDefault(ws => ws.AthleteId == athlete.Id);
            if (weeklyStats is null) continue;

            game.HomeScore += weeklyStats.Receptions;
            game.HomeScore += (weeklyStats.ReceivingTouchdowns + weeklyStats.PassingTouchdowns + weeklyStats.RushingTouchdowns) * 6;
            game.HomeScore += (weeklyStats.ReceivingYards + weeklyStats.PassingYards + weeklyStats.RushingYards) / 10;
        }
        foreach (var athlete in game.Away.Athletes)
        {
            var weeklyStats = game.WeeklyStats.FirstOrDefault(ws => ws.AthleteId == athlete.Id);
            if (weeklyStats is null) continue;

            game.AwayScore += weeklyStats.Receptions;
            game.AwayScore += (weeklyStats.ReceivingTouchdowns + weeklyStats.PassingTouchdowns + weeklyStats.RushingTouchdowns) * 6;
            game.AwayScore += (weeklyStats.ReceivingYards + weeklyStats.PassingYards + weeklyStats.RushingYards) / 10;
        }

        return game;
    }
}