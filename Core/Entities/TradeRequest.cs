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
}