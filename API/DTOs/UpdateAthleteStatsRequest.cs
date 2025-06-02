namespace API.DTOs;

public class UpdateAthleteStatsRequest
{
    public int Week { get; set; }
    public int Season { get; set; }
    public StatsDto Stats { get; set; }
}