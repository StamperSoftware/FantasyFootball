using System.Diagnostics.CodeAnalysis;

namespace Core.Entities;

public class Game : BaseEntity
{
    public int HomeId { get; set; }
    public required UserTeam Home { get; set; }
    public int AwayId { get; set; }
    public required UserTeam Away { get; set; }
    public int Week { get; set; }
    public int Season { get; set; }

    public int? WinnerId { get; set; }
    public UserTeam? Winner { get; set; }
    public int? LoserId { get; set; }
    public UserTeam? Loser { get; set; }

    public int? WinningScore { get; set; }
    public int? LosingScore { get; set; }

    public int ScheduleId { get; set; }

    public override string ToString()
    {
        return $"Home: {Home.Name} vs Away: {Away.Name}";
    }

    public Game()
    {
    }

    [SetsRequiredMembers]
    public Game(UserTeam home, UserTeam away, int week)
    {
        Home = home;
        Away = away;
        Week = week;
    }

}
