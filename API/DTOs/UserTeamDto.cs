using System.Text.Json.Serialization;
using Core.Entities;

namespace API.DTOs;

public class UserTeamDto
{
    public int Id { get; set; }
    
    public int LeagueId { get; set; }
    
    public required string UserId { get; set; }
    
    public string? Name { get; set; }

    [JsonIgnore] public string RosterId { get; set; } = "";
    public Roster? Roster { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Ties { get; set; }
}