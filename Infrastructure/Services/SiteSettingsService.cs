using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data.MongoDb;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Services;

public class SiteSettingsService:ISiteSettingsService
{

    public SiteSettingsService(IOptions<DbSettings> dbSettings)
    {
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
        _siteSettings = mongoDb.GetCollection<SiteSettings>(dbSettings.Value.SiteSettings);
    }
    
    
    private readonly IMongoCollection<SiteSettings> _siteSettings;
    
    public async Task<SiteSettings> GetSettings()
    {
        var settings = await _siteSettings.Find(s => s.Id == 0).FirstOrDefaultAsync();
        if (settings is null)
        {
            settings = new SiteSettings
            {
                CurrentSeason = 2025,
                CurrentWeek = 0,
                Id = 0,
            };
            await _siteSettings.InsertOneAsync(settings);
        }

        return settings;
    }

    public async Task UpdateSettings(SiteSettings settings)
    {
        await _siteSettings.ReplaceOneAsync(s => s.Id == settings.Id, settings);
    }
}