using System.Globalization;
using Core.Entities;
using Core.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class StatisticsService(FantasyFootballContext dbContext, ISiteSettingsService siteSettingsService):IStatisticsService
{
    
    
    public async Task UploadWeeklyStats(MemoryStream stream)
    {
        var siteSettings = await siteSettingsService.GetSettings();
        var currentStats = dbContext.AthleteWeeklyStats.Where(aws => aws.Week == siteSettings.CurrentWeek && aws.Season == siteSettings.CurrentSeason);
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var csvRecords = csv.GetRecords<UploadAthleteStatistics>();
        IList<AthleteWeeklyStats> parsedRecords = [];
        
        foreach (var record in csvRecords)
        {
            var parsedRecord = await currentStats.FirstOrDefaultAsync(aws => aws.AthleteId == record.AthleteId) ?? new AthleteWeeklyStats(siteSettings.CurrentWeek, siteSettings.CurrentSeason, record.AthleteId);
            
            parsedRecord.Receptions = record.Receptions;
            parsedRecord.ReceivingTouchdowns = record.ReceivingTouchdowns;
            parsedRecord.ReceivingYards = record.ReceivingYards;
            parsedRecord.PassingTouchdowns = record.PassingTouchdowns;
            parsedRecord.PassingYards = record.PassingYards;
            parsedRecord.RushingTouchdowns = record.RushingTouchdowns;
            parsedRecord.RushingYards = record.RushingYards;

            if (parsedRecord.Id == null)
            {
                parsedRecords.Add(parsedRecord);
            }
            else
            {
                dbContext.AthleteWeeklyStats.Update(parsedRecord);
            }

        }

        if (parsedRecords.Count > 0)
        {
            await dbContext.AthleteWeeklyStats.AddRangeAsync(parsedRecords);
        }
        await dbContext.SaveChangesAsync();
    }
    
}