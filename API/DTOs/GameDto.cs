using System.Diagnostics.CodeAnalysis;
using Core.Entities;

namespace API.DTOs;

[method: SetsRequiredMembers]
public class GameDto()
{
    public int Id { get; set; }
    public int HomeId { get; set; } 
    public required UserTeamDto Home { get; set; }
    public int AwayId { get; set; }
    public required UserTeamDto Away { get; set; }
    public int Week { get; set; }
    public int Season { get; set; }
    public IList<AthleteWeeklyStatsDto>? WeeklyStats { get; set; }
    public int? HomeScore { get; set; }
    public int? AwayScore { get; set; }
    public bool IsFinalized { get; set; }
}