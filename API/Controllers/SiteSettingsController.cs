using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/site-settings")]
public class SiteSettingsController(ISiteSettingsService siteSettingsService):BaseApiController
{
    [HttpGet]
    public async Task<SiteSettings> GetSiteSettings()
    {
        return await siteSettingsService.GetSettings();
    }   
    [HttpPut]
    public async Task UpdateSiteSettings(SiteSettings updateSiteSettingsDto)
    {
        await siteSettingsService.UpdateSettings(updateSiteSettingsDto);
    }   
}