using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.MongoDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Services;

public class GameService:IGameService
{
    private readonly IMongoCollection<Game> _games;
    private readonly FantasyFootballContext _db;
    private readonly IRosterService _rosterService;
    private readonly ISiteSettingsService _siteSettingsService;
    
    public GameService(IOptions<DbSettings> dbSettings,FantasyFootballContext db, IRosterService rosterService, ISiteSettingsService siteSettingsService)
    {
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
        _games = mongoDb.GetCollection<Game>(dbSettings.Value.Games);
        _db = db;
        _rosterService = rosterService;
        _siteSettingsService = siteSettingsService;
    }

    
    public async Task<Game?> GetFullDetailAsync(int gameId)
    {
        var game = await _db.Games
            .Include(g => g.Away)
                .ThenInclude(u => u.Player)
                    .ThenInclude(p => p.User)
            .Include(g => g.Home)
                .ThenInclude(u => u.Player)
                    .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(g => g.Id == gameId);

        if (game == null) throw new Exception("Could not get game");
        
        var homeRoster = await _rosterService.GetRoster(game.Home.RosterId) ?? throw new Exception("Could not get roster");
        game.Home.Roster = homeRoster;
        var awayRoster = await _rosterService.GetRoster(game.Away.RosterId) ?? throw new Exception("Could not get roster");
        game.Away.Roster = awayRoster;
        var athleteIds = homeRoster.Starters.Union(homeRoster.Bench).Union(awayRoster.Starters).Union(awayRoster.Bench)
            .Select(a => a.Id);

        game.WeeklyStats = await _db.AthleteWeeklyStats.Where(aws => athleteIds.Contains(aws.AthleteId))
            .Where(aws => aws.Season == game.Season).Where(aws => aws.Week == game.Week).ToListAsync();
        
        return game;
    }


    public async Task<Game?> GetGame(int gameId)
    {
        if (await _db.Games.AnyAsync(g => g.Id == gameId && g.IsFinalized))
        {
            return await _games.Find(g => g.Id == gameId).FirstOrDefaultAsync();
        }

        return await GetFullDetailAsync(gameId) ?? throw new Exception("Could not get game");
    }


    public async Task UpdateScoreAsync(int gameId)
    {
        var game = await GetFullDetailAsync(gameId) ?? throw new Exception("Could not get game");

        if (game.Away.Roster is null || game.Home.Roster is null) throw new Exception("Could not get rosters");
        
        var homeStarterIds = game.Home.Roster.Starters.Select(a => a.Id);
        var awayStarterIds = game.Away.Roster.Starters.Select(a => a.Id);

        var weeklyStats = _db.AthleteWeeklyStats
            .Where(aws => aws.Season == game.Season)
            .Where(aws => aws.Week == game.Week)
            .Where(aws => homeStarterIds.Union(awayStarterIds).Contains(aws.AthleteId));
        
        var homeStats = weeklyStats.Where(aws => homeStarterIds.Contains(aws.AthleteId));
        var awayStats = weeklyStats.Where(aws => awayStarterIds.Contains(aws.AthleteId));
        
        game.HomeScore = _calculateScore(homeStats);
        game.AwayScore = _calculateScore(awayStats);
        
        await _db.SaveChangesAsync();
    }
    
    public async Task FinalizeGameAsync(int gameId)
    {
        var game = await GetFullDetailAsync(gameId) ?? throw new Exception("Could not get Game");
        _finalizeGameAsync(game);
        await _games.InsertOneAsync(game);
        await _db.SaveChangesAsync();
    }

    public async Task FinalizeGamesAsync()
    {
        var siteSettings = await _siteSettingsService.GetSettings();
        
        var games = await _db.Games
            .Where(g => g.Season == siteSettings.CurrentSeason)
            .Where(g => g.Week == siteSettings.CurrentWeek)
                .Include(g => g.Away)
                    .ThenInclude(u => u.Player)
                        .ThenInclude(p => p.User)
                .Include(g => g.Home)
                    .ThenInclude(u => u.Player)
                        .ThenInclude(p => p.User)
            .ToListAsync();

        foreach (var game in games)
        {
            var homeRoster = await _rosterService.GetRoster(game.Home.RosterId) ?? throw new Exception("Could not get roster");
            game.Home.Roster = homeRoster;
            var awayRoster = await _rosterService.GetRoster(game.Away.RosterId) ?? throw new Exception("Could not get roster");
            game.Away.Roster = awayRoster;
            var athleteIds = homeRoster.Starters.Union(homeRoster.Bench).Union(awayRoster.Starters).Union(awayRoster.Bench)
                .Select(a => a.Id);
            var homeStarterIds = homeRoster.Starters.Select(a => a.Id);
            var awayStarterIds = awayRoster.Starters.Select(a => a.Id);
            game.WeeklyStats = await _db.AthleteWeeklyStats.Where(aws => athleteIds.Contains(aws.AthleteId))
                .Where(aws => aws.Season == game.Season).Where(aws => aws.Week == game.Week).ToListAsync();

            game.HomeScore = _calculateScore(game.WeeklyStats.Where(aws => homeStarterIds.Contains(aws.AthleteId)));
            game.AwayScore = _calculateScore(game.WeeklyStats.Where(aws => awayStarterIds.Contains(aws.AthleteId)));
            
            _finalizeGameAsync(game);
        }

        await _games.InsertManyAsync(games);
        await _db.SaveChangesAsync();
    }
    
    private void _finalizeGameAsync(Game game)
    {
        if (game.IsFinalized) throw new Exception("Game is already finalized");
        game.FinalizeGame();
        
        if (game.HomeScore > game.AwayScore)
        {
            game.Home.AddWin();
            game.Away.AddLoss();
        } else if (game.AwayScore > game.HomeScore)
        {
            game.Home.AddLoss();
            game.Away.AddWin();
        }
        else
        {
            game.Home.AddTie();
            game.Away.AddTie();
        }
    }

    private int _calculateScore(IEnumerable<AthleteWeeklyStats> stats)
    {
        var score = 0;
        foreach (var stat in stats)
        {
            score += stat.Receptions;
            score += (stat.ReceivingTouchdowns + stat.PassingTouchdowns + stat.RushingTouchdowns) * 6;
            score += (stat.ReceivingYards + stat.PassingYards + stat.RushingYards) / 10;
        }
        return score;

    }
    
}