using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data.MongoDb;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Services;

public class SiteSettingsService:ISiteSettingsService
{
    private readonly IMongoCollection<SiteSettings> _siteSettings;

    public SiteSettingsService(IOptions<DbSettings> dbSettings)
    {
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
        _siteSettings = mongoDb.GetCollection<SiteSettings>(dbSettings.Value.SiteSettings);
    }
    
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

    public async Task<SiteSettings> AdvanceWeek()
    {
        var current = await _siteSettings.Find(s => s.Id == 0).FirstOrDefaultAsync() ?? throw new Exception("Could not get settings");
        current.CurrentWeek++;
        await _siteSettings.ReplaceOneAsync(s => s.Id == 0, current);
        return current;
    }
}