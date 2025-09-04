using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data.MongoDb;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Services;

public class LeagueSettingsService:ILeagueSettingsService
{
    private readonly IMongoCollection<LeagueSettings> _leagueSettings;

    public LeagueSettingsService(IOptions<DbSettings> dbSettings)
    {
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
        _leagueSettings = mongoDb.GetCollection<LeagueSettings>(dbSettings.Value.LeagueSettings);
    }
    
    public async Task<LeagueSettings?> GetLeagueSettings(int leagueId)
    {
        return await _leagueSettings.Find(ls => ls.LeagueId == leagueId).FirstOrDefaultAsync() ?? throw new Exception("Could not get league settings");
    }

    public async Task UpdateLeagueSettings(LeagueSettings updateLeagueSettings)
    {
        await _leagueSettings.ReplaceOneAsync(ls => ls.LeagueId == updateLeagueSettings.LeagueId, updateLeagueSettings);
    }
    
    public async Task<LeagueSettings> CreateLeagueSettings(int leagueId)
    {
        var settings = new LeagueSettings(leagueId);
        
        await _leagueSettings.InsertOneAsync(settings);
        return settings;
    }

    public async Task DeleteLeagueSettings(int leagueId)
    {
        await _leagueSettings.DeleteOneAsync(l => l.LeagueId == leagueId);
    }
}