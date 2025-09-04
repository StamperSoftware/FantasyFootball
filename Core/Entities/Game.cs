using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Game : BaseEntity
{
    public int HomeId { get; set; }
    [NotMapped] public required UserTeam Home { get; set; }
    public int AwayId { get; set; }
    [NotMapped] public required UserTeam Away { get; set; }
    public int Week { get; set; }
    public int Season { get; set; }
    [NotMapped] public IList<AthleteWeeklyStats> WeeklyStats { get; set; } = [];
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }
    public bool IsFinalized { get; set; }
    
    public override string ToString() => $"Home: {Home.Name} ({HomeScore}) vs Away: {Away.Name} ({AwayScore})";
    
    public static Game CreateGame(UserTeam home, UserTeam away, int week, int season)
    {
        return new Game
        {
            Home = home,
            HomeId = home.Id,
            Away = away,
            AwayId = away.Id,
            Week = week,
            Season = season,
        };
    }
    
    public void FinalizeGame()
    {
        IsFinalized = true;
    }
    
}
