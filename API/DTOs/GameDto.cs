using System.Diagnostics.CodeAnalysis;
using Core.Entities;

namespace API.DTOs;

public class GameDto
{
    public int Id { get; set; }
    public int HomeId { get; set; }
    public required UserTeamDto Home { get; set; }
    public int AwayId { get; set; }
    public required UserTeamDto Away { get; set; }
    public int Week { get; set; }
    
    
    public int? WinnerId { get; set; }
    public UserTeamDto? Winner { get; set; }
    public int? LoserId { get; set; }
    public UserTeamDto? Loser { get; set; }
    
    public int? WinningScore { get; set; }
    public int? LosingScore { get; set; }
    
    public int ScheduleId { get; set; }

    public GameDto(){}

    [SetsRequiredMembers]
    public GameDto(Game game)
    {
        Id = game.Id;
        HomeId = game.HomeId;
        Home = new UserTeamDto(game.Home);
        AwayId = game.AwayId;
        Away = new UserTeamDto(game.Away);
        Week = game.Week;
        ScheduleId = game.ScheduleId;
    }
    
}