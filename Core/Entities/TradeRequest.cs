using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Entities;

public class TradeRequest
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public int LeagueId { get; set; }
    public required int InitiatingTeamId { get; set; }
    public required int ReceivingTeamId { get; set; }
    public IList<int> InitiatingAthleteIds { get; set; } = [];
    public IList<int> ReceivingAthleteIds { get; set; } = [];
    public IList<Athlete> ReceivingAthletes { get; set; } = [];
    public IList<Athlete> InitiatingAthletes { get; set; } = [];

    public static TradeRequest Create(int leagueId, UserTeam initiatingTeam, UserTeam receivingTeam, IList<int> receivingAthleteIds, IList<int> initiatingAthleteIds)
    {
        return new TradeRequest
        {
            LeagueId = leagueId,
            InitiatingTeamId = initiatingTeam.Id,
            ReceivingTeamId = receivingTeam.Id,
            ReceivingAthleteIds = receivingAthleteIds,
            InitiatingAthleteIds = initiatingAthleteIds,
            ReceivingAthletes = receivingTeam.Roster.Starters.Union(receivingTeam.Roster.Bench).Where(a => receivingAthleteIds.Contains(a.Id)).ToList(),
            InitiatingAthletes = initiatingTeam.Roster.Starters.Union(initiatingTeam.Roster.Bench).Where(a => initiatingAthleteIds.Contains(a.Id)).ToList(),
        };
    }
    
}