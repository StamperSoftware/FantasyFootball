using Core.Entities;

namespace API.DTOs;

public class LeagueDto
{
    public int Id { get; set; }
    public IList<UserTeamDto> Teams { get; set; }
    public string Name { get; set; }

    public LeagueDto(League league)
    {
        Id = league.Id;
        Teams = league.Teams.Select(team => new UserTeamDto(team)).ToList();
        Name = league.Name;
    }
    
}