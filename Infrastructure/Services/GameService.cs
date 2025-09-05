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
    private readonly ISiteSettingsService _siteSettingsService;
    private readonly ILeagueSettingsService _leagueSettingsService;
    
    public GameService(IOptions<DbSettings> dbSettings, FantasyFootballContext db, ISiteSettingsService siteSettingsService, ILeagueSettingsService leagueSettingsService)
    {
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
        _games = mongoDb.GetCollection<Game>(dbSettings.Value.Games);
        _db = db;
        _siteSettingsService = siteSettingsService;
        _leagueSettingsService = leagueSettingsService;
    }
    
    public async Task<Game?> GetFullDetailAsync(string gameId)
    {
        var game = await _games.Find(g => g.Id == gameId).FirstOrDefaultAsync() ?? throw new Exception("Could not get game");
        var leagueSettings = await _leagueSettingsService.GetLeagueSettings(game.LeagueId) ?? throw new Exception("Could not get league settings");
        
        var athleteIds = game.Home.Roster.Starters.Union(game.Home.Roster.Bench).Union(game.Away.Roster.Starters).Union(game.Away.Roster.Bench)
            .Select(a => a.Id);

        game.WeeklyStats = await _db.AthleteWeeklyStats.Where(aws => athleteIds.Contains(aws.AthleteId))
            .Where(aws => aws.Season == game.Season).Where(aws => aws.Week == game.Week).ToListAsync();
        
        var homeStarterIds = game.Home.Roster.Starters.Select(a => a.Id);
        var awayStarterIds = game.Away.Roster.Starters.Select(a => a.Id);

        var homeStats = game.WeeklyStats.Where(aws => homeStarterIds.Contains(aws.AthleteId));
        var awayStats = game.WeeklyStats.Where(aws => awayStarterIds.Contains(aws.AthleteId));
        
        game.HomeScore = _calculateScore(homeStats, leagueSettings);
        game.AwayScore = _calculateScore(awayStats, leagueSettings);
        
        return game;
    }

    public async Task<Game?> GetGame(string gameId)
    {
        return await GetFullDetailAsync(gameId);
    }

    public async Task UpdateScoreAsync(string gameId)
    {
        var game = await GetFullDetailAsync(gameId) ?? throw new Exception("Could not get game");
        if (game.IsFinalized) throw new Exception("Game is final");
        if (game.Away.Roster is null || game.Home.Roster is null) throw new Exception("Could not get rosters");
        
        var leagueSettings = await _leagueSettingsService.GetLeagueSettings(game.LeagueId) ?? throw new Exception("Could not get league settings");
        var homeStarterIds = game.Home.Roster.Starters.Select(a => a.Id);
        var awayStarterIds = game.Away.Roster.Starters.Select(a => a.Id);

        var weeklyStats = _db.AthleteWeeklyStats
            .Where(aws => aws.Season == game.Season)
            .Where(aws => aws.Week == game.Week)
            .Where(aws => homeStarterIds.Union(awayStarterIds).Contains(aws.AthleteId));
        
        var homeStats = weeklyStats.Where(aws => homeStarterIds.Contains(aws.AthleteId));
        var awayStats = weeklyStats.Where(aws => awayStarterIds.Contains(aws.AthleteId));

        game.HomeScore = _calculateScore(homeStats, leagueSettings);
        game.AwayScore = _calculateScore(awayStats, leagueSettings);
    }
    
    public async Task FinalizeGameAsync(string gameId)
    {
        var game = await GetFullDetailAsync(gameId) ?? throw new Exception("Could not get Game");
        _finalizeGame(game);
        await _games.ReplaceOneAsync(g=>g.Id == game.Id,game);
    }

    public async Task FinalizeGamesAsync()
    {
        var siteSettings = await _siteSettingsService.GetSettings();
        
        var games = await _games.Find(g =>
            g.Season == siteSettings.CurrentSeason && g.Week == siteSettings.CurrentWeek && !g.IsFinalized).ToListAsync();
        
        foreach (var game in games)
        {
            var leagueSettings = await _leagueSettingsService.GetLeagueSettings(game.LeagueId) ?? throw new Exception("Could not get league settings");
            var athleteIds = game.Home.Roster.Starters.Union(game.Home.Roster.Bench).Union(game.Away.Roster.Starters).Union(game.Away.Roster.Bench)
                .Select(a => a.Id);
            var homeStarterIds = game.Home.Roster.Starters.Select(a => a.Id);
            var awayStarterIds = game.Away.Roster.Starters.Select(a => a.Id);
            
            game.WeeklyStats = await _db.AthleteWeeklyStats.Where(aws => athleteIds.Contains(aws.AthleteId))
                .Where(aws => aws.Season == game.Season).Where(aws => aws.Week == game.Week).ToListAsync();

            var homeStats = game.WeeklyStats.Where(aws => homeStarterIds.Contains(aws.AthleteId));
            var awayStats = game.WeeklyStats.Where(aws => awayStarterIds.Contains(aws.AthleteId));
            
            game.HomeScore = _calculateScore(homeStats, leagueSettings);
            game.AwayScore = _calculateScore(awayStats, leagueSettings);
            
            _finalizeGame(game);
            await _games.ReplaceOneAsync(g=>g.Id == game.Id, game);
        }
        
        await _db.SaveChangesAsync();
    }

    public async Task DeleteGames(IEnumerable<string> gameIds)
    {
        await _games.DeleteManyAsync(g => gameIds.Contains(g.Id));
    }

    public async Task<List<Game>> GetUserGames(int userId)
    {
       return await _games.Find(g => g.HomeId == userId || g.AwayId == userId).ToListAsync();
    } 
    
    public async Task<List<Game>> GetLeagueGames(int leagueId)
    {
       var games = await _games.Find(g => g.LeagueId == leagueId).ToListAsync();
       foreach (var game in games)
       {
           var leagueSettings = await _leagueSettingsService.GetLeagueSettings(game.LeagueId) ?? throw new Exception("Could not get league settings");
           var athleteIds = game.Home.Roster.Starters.Union(game.Home.Roster.Bench).Union(game.Away.Roster.Starters).Union(game.Away.Roster.Bench)
               .Select(a => a.Id);
           var homeStarterIds = game.Home.Roster.Starters.Select(a => a.Id);
           var awayStarterIds = game.Away.Roster.Starters.Select(a => a.Id);
            
           game.WeeklyStats = await _db.AthleteWeeklyStats.Where(aws => athleteIds.Contains(aws.AthleteId))
               .Where(aws => aws.Season == game.Season).Where(aws => aws.Week == game.Week).ToListAsync();

           var homeStats = game.WeeklyStats.Where(aws => homeStarterIds.Contains(aws.AthleteId));
           var awayStats = game.WeeklyStats.Where(aws => awayStarterIds.Contains(aws.AthleteId));
            
           game.HomeScore = _calculateScore(homeStats, leagueSettings);
           game.AwayScore = _calculateScore(awayStats, leagueSettings);
       }

       return games;
    }

    public async Task AddGames(IList<Game> games)
    {
        await _games.InsertManyAsync(games);
    }

    private void _finalizeGame(Game game)
    {
        if (game.IsFinalized) throw new Exception("Game is already finalized");
        game.FinalizeGame();
        var home = _db.UserTeams.Find(game.HomeId) ?? throw new Exception("Could not get home team");
        var away = _db.UserTeams.Find(game.AwayId) ?? throw new Exception("Could not get away team");
        if (game.HomeScore > game.AwayScore)
        {
            home.AddWin();
            away.AddLoss();
        } else if (game.AwayScore > game.HomeScore)
        {
            home.AddLoss();
            away.AddWin();
        }
        else
        {
            home.AddTie();
            away.AddTie();
        }
    }

    private double _calculateScore(IEnumerable<AthleteWeeklyStats> stats, LeagueSettings settings)
    {
        var score = 0.0;
        foreach (var stat in stats)
        {
            score += (stat.Receptions * settings.ReceptionScore);
            score += (stat.ReceivingTouchdowns * settings.ReceivingTouchdownsScore)+ (stat.PassingTouchdowns*settings.PassingTouchdownsScore) + (stat.RushingTouchdowns*settings.RushingTouchdownsScore);
            score += (stat.ReceivingYards * settings.ReceivingYardsScore) + (stat.PassingYards*settings.PassingYardsScore) + (stat.RushingYards*settings.RushingYardsScore);
        }
        return Math.Round(score,2);

    }
}