using Core.Entities;

namespace API.DTOs;

public class UserTeamDto
{
    public int Id { get; set; }
    
    public int LeagueId { get; set; }
    
    public PlayerDto Player { get; set; }
    
    public string? Name { get; set; }

    public string RosterId { get; set; }
    public Roster? Roster { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Ties { get; set; }

    public UserTeamDto(UserTeam team)
    {
        Id = team.Id;
        LeagueId = team.LeagueId;
        Player = new PlayerDto(team.Player);
        Name = team.Name;
        RosterId = team.RosterId;
        Roster = team.Roster;
        Wins = team.Wins;
        Losses = team.Losses;
        Ties = team.Ties;
    }
    
}