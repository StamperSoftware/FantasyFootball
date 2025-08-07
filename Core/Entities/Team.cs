using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Entities;

public class Team : BaseEntity
{
    
    public string Name { get; set; } = "";
    public string Location { get; set; } = "";
    [NotMapped]
    [JsonIgnore]
    public IList<Athlete> Athletes { get; set; } = [];
}