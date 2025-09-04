using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Entities;

public class Roster
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public IList<Athlete> Starters { get; set; } = [];
    public IList<Athlete> Bench { get; set; } = [];
}