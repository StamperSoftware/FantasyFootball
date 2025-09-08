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
    public int Ties { get; set; }
    public required string RosterId { get; set; } = "";
    
    [NotMapped] public Roster Roster { get; set; } = new();
    [NotMapped] public bool IsOnline { get; set; }
    [NotMapped] public IList<Game> Schedule { get; set; } = [];
    
    public UserTeam(){}

    [SetsRequiredMembers]
    public UserTeam(int leagueId, Player player, int wins, int losses, int ties, string name)
    {
        LeagueId = leagueId;
        Player = player;
        PlayerId = player.Id;
        Name = name;
        Wins = wins;
        Losses = losses;
        Ties = ties;
    }

    public void AddWin()
    {
        Wins++;
    }

    public void AddLoss()
    {
        Losses++;
    }

    public void AddTie()
    {
        Ties++;
    }

    public void SetOnline()
    {
        IsOnline = true;
    }
    public void SetOffline()
    {
        IsOnline = false;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;
        return Id == ((UserTeam)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}