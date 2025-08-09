using Core.Entities;

namespace Core.Interfaces;

public interface ISiteSettingsService
{
    public Task<SiteSettings> GetSettings();
    public Task UpdateSettings(SiteSettings settings);
}