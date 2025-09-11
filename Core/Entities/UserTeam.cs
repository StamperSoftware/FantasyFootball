using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Core.Entities;

public class UserTeam : BaseEntity
{
    public int LeagueId { get; set; }
    public int PlayerId { get; set; }
    public Player Player { get; set; } = null!;
    public string? Name { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Ties { get; set; }
    public required string RosterId { get; set; } = "";
    
    [NotMapped] public Roster Roster { get; set; } = new();
    [NotMapped] public bool IsOnline { get; set; }
    [NotMapped] public IList<Game> Schedule { get; set; } = [];

    public static UserTeam CreateNewTeam(int leagueId, int playerId, string name)
    {
        return new UserTeam
        {
            LeagueId = leagueId,
            PlayerId = playerId,
            RosterId = "",
            Wins = 0,
            Losses = 0,
            Ties = 0,
            Name = name
        };
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