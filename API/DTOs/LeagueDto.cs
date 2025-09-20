using Core.Entities;

namespace API.DTOs;

public class LeagueDto
{
    public int Id { get; set; }
    public IList<UserTeamDto> Teams { get; set; } = [];
    public required string Name { get; set; }
    public IList<GameDto>? Schedule { get; set; }
    public int Season { get; set; }
    public required LeagueSettings Settings { get; set; }
    public required string AdminId { get; set; }
}