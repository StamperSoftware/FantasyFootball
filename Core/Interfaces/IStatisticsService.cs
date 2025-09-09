namespace Core.Interfaces;

public interface IStatisticsService
{
    public Task UploadWeeklyStats(MemoryStream stream);
}