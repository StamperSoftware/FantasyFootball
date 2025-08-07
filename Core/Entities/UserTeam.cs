using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Core.Entities;

public class UserTeam : BaseEntity
{
    public int LeagueId { get; set; }
    public int PlayerId { get; set; }
    public required Player Player { get; set; }
    public string? Name { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public required string RosterId { get; set; }
    [NotMapped]
    public Roster? Roster { get; set; }
    
    public UserTeam(){}

    [SetsRequiredMembers]
    public UserTeam(int leagueId, Player player, int wins, int losses, string name)
    {
        LeagueId = leagueId;
        Player = player;
        PlayerId = player.Id;
        Name = name;
        Wins = wins;
        Losses = losses;
    }
}