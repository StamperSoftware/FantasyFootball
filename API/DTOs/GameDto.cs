using System.Diagnostics.CodeAnalysis;
using Core.Entities;

namespace API.DTOs;

[method: SetsRequiredMembers]
public class GameDto(Game game)
{
    public int Id { get; set; } = game.Id;
    public int HomeId { get; set; } = game.HomeId;
    public required UserTeamDto Home { get; set; } = new(game.Home);
    public int AwayId { get; set; } = game.AwayId;
    public required UserTeamDto Away { get; set; } = new(game.Away);
    public int Week { get; set; } = game.Week;
    public int Season { get; set; } = game.Season;
    public IList<AthleteWeeklyStatsDto> WeeklyStats { get; set; } = game.WeeklyStats.Select(ws => new AthleteWeeklyStatsDto(ws)).ToList();
    public int? HomeScore { get; set; } = game.HomeScore;
    public int? AwayScore { get; set; } = game.AwayScore;
    public bool IsFinalized { get; set; } = game.IsFinalized;
}