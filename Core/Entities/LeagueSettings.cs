using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Entities;

public class LeagueSettings
{
    [BsonId]
    public int LeagueId { get; set; } 
    public int NumberOfGames { get; set; } 
    public int NumberOfTeams { get; set; } 
    public int RosterLimit { get; set; } 
    public int StartingQuarterBackLimit { get; set; } 
    public int StartingRunningBackLimit { get; set; } 
    public int StartingWideReceiverLimit { get; set; } 
    public int StartingTightEndLimit { get; set; } 
    public int FlexLimit { get; set; } 
    public int ReceptionScore { get; set; } 
    public double ReceivingYardsScore { get; set; } 
    public int ReceivingTouchdownsScore { get; set; } 
    public double RushingYardsScore { get; set; } 
    public int RushingTouchdownsScore { get; set; } 
    public double PassingYardsScore { get; set; }
    public int PassingTouchdownsScore { get; set; } 
    public IList<int> DraftOrder { get; set; } = [];

    public static LeagueSettings CreateLeagueSettings(int leagueId)
    {
        return new LeagueSettings
        {
            LeagueId = leagueId,
            NumberOfGames = 12,
            NumberOfTeams = 10,
            RosterLimit = 15,
            StartingQuarterBackLimit = 1,
            StartingRunningBackLimit = 2,
            StartingWideReceiverLimit = 2,
            StartingTightEndLimit = 1,
            FlexLimit = 0,
            ReceptionScore = 1,
            ReceivingYardsScore = .1,
            ReceivingTouchdownsScore = 6,
            RushingYardsScore = .1,
            RushingTouchdownsScore = 6,
            PassingYardsScore = .1,
            PassingTouchdownsScore = 6,
        };
    }
}

