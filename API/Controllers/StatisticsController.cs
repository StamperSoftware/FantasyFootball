using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class StatisticsController(IStatisticsService statisticsService):BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> UploadStatistics(IFormFile file)
    {
        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            stream.Position = 0;
            await statisticsService.UploadWeeklyStats(stream);
        }

        return Ok();
    }
}