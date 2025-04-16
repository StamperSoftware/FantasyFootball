using Core.Entities;

namespace API.DTOs;

public class UserTeamDto
{
    public int Id { get; set; }
    public int LeagueId { get; set; }
    public PlayerDto Player { get; set; }
    
    public string? Name { get; set; }

    public UserTeamDto(UserTeam team)
    {
        Id = team.Id;
        LeagueId = team.LeagueId;
        Player = new PlayerDto(team.Player);
        Name = team.Name;
    }
    
}