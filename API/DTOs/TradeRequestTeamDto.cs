namespace API.DTOs;

public class TradeRequestTeamDto
{
    public IList<AthleteDto> MyPlayers { get; set; } = [];
    public IList<AthleteDto> TheirPlayers { get; set; } = [];
    public required string Id { get; set; }
}