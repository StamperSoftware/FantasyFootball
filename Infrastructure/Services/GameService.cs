using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class GameService(FantasyFootballContext db, IRosterService rosterService):IGameService
{
    public async Task<Game?> GetFullDetailAsync(int gameId)
    {

        var game = await db.Games
            .Include(g => g.Away)
                .ThenInclude(u => u.Player)
                    .ThenInclude(p => p.User)
            .Include(g => g.Home)
                .ThenInclude(u => u.Player)
                    .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(g => g.Id == gameId);

        if (game == null) throw new Exception("Could not get game");
        
        var homeRoster = await rosterService.GetRoster(game.Home.RosterId) ?? throw new Exception("Could not get roster");
        var homeAthletes = homeRoster.Starters.Union(homeRoster.Bench);
        game.Home.Roster = homeRoster;
        var awayRoster = await rosterService.GetRoster(game.Away.RosterId) ?? throw new Exception("Could not get roster");
        var awayAthletes = awayRoster.Starters.Union(homeRoster.Bench);
        game.Away.Roster = awayRoster;
        
        var athleteIds = homeAthletes.Union(awayAthletes).Select(a => a.Id).ToList();

        game.WeeklyStats = await db.AthleteWeeklyStats
            .Where(aws => aws.Season == 2025)
            .Where(aws => aws.Week == game.Week)
            .Where(aws => athleteIds.Contains(aws.AthleteId))
            .ToListAsync();

        foreach (var athlete in homeRoster.Starters)
        {
            var weeklyStats = game.WeeklyStats.FirstOrDefault(ws => ws.AthleteId == athlete.Id);
            if (weeklyStats is null) continue;

            game.HomeScore += weeklyStats.Receptions;
            game.HomeScore += (weeklyStats.ReceivingTouchdowns + weeklyStats.PassingTouchdowns + weeklyStats.RushingTouchdowns) * 6;
            game.HomeScore += (weeklyStats.ReceivingYards + weeklyStats.PassingYards + weeklyStats.RushingYards) / 10;
        }
        foreach (var athlete in awayRoster.Starters)
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