namespace API.DTOs;

public class GameDto
{
    public required string Id { get; set; }
    public int HomeId { get; set; } 
    public required UserTeamDto Home { get; set; }
    public int AwayId { get; set; }
    public required UserTeamDto Away { get; set; }
    public int Week { get; set; }
    public int Season { get; set; }
    public IList<AthleteWeeklyStatsDto>? WeeklyStats { get; set; }
    public double? HomeScore { get; set; }
    public double? AwayScore { get; set; }
    public bool IsFinalized { get; set; }
}