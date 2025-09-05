using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Entities;

public class Game
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public int HomeId { get; set; }
    [NotMapped] public required UserTeam Home { get; set; }
    public int AwayId { get; set; }
    [NotMapped] public required UserTeam Away { get; set; }
    public int Week { get; set; }
    public int Season { get; set; }
    [NotMapped] public IList<AthleteWeeklyStats> WeeklyStats { get; set; } = [];
    public double HomeScore { get; set; }
    public double AwayScore { get; set; }
    public bool IsFinalized { get; set; }
    [JsonIgnore] public int LeagueId { get; set; }
    
    public override string ToString() => $"Home: {Home.Name} ({HomeScore}) vs Away: {Away.Name} ({AwayScore})";
    
    public static Game CreateGame(UserTeam home, UserTeam away, int week, int season, int leagueId)
    {
        return new Game
        {
            Home = home,
            HomeId = home.Id,
            Away = away,
            AwayId = away.Id,
            Week = week,
            Season = season,
            LeagueId = leagueId
        };
    }
    
    public void FinalizeGame()
    {
        IsFinalized = true;
    }
    
}
