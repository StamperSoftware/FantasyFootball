using Core.Entities;

namespace API.DTOs;

public class LeagueDto
{
    public int Id { get; set; }
    public IList<UserTeam> Teams { get; set; } = [];
    public string Name { get; set; } = "";
}