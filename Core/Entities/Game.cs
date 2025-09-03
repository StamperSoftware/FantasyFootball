using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Core.Entities;

public class Game : BaseEntity
{
    public int HomeId { get; set; }
    [NotMapped]
    public required UserTeam Home { get; set; }
    public int AwayId { get; set; }
    [NotMapped]
    public required UserTeam Away { get; set; }
    public int Week { get; set; }
    public int Season { get; set; }
    [NotMapped]
    public IList<AthleteWeeklyStats> WeeklyStats { get; set; } = [];
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }
    public bool IsFinalized { get; set; }
    
    public override string ToString() => $"Home: {Home.Name} ({HomeScore}) vs Away: {Away.Name} ({AwayScore})";
    
    public Game(){}

    [SetsRequiredMembers]
    public Game(UserTeam home, UserTeam away, int week, int season, bool isFinalized)
    {
        Home = home;
        Away = away;
        Week = week;
        Season = season;
        IsFinalized = isFinalized;
    }

    public void FinalizeGame()
    {
        IsFinalized = true;
    }
    
}
