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
    public IList<AthleteWeeklyStats> WeeklyStats { get; set; } = [];
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }

    public int ScheduleId { get; set; }

    public override string ToString() => $"Home: {Home.Name} ({HomeScore})vs Away: {Away.Name} ({AwayScore})";

    public Game(){}

    [SetsRequiredMembers]
    public Game(UserTeam home, UserTeam away, int week)
    {
        Home = home;
        Away = away;
        Week = week;
        Season = 2025;
    }

}
