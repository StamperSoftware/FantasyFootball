namespace Core.Entities;

public class League : BaseEntity
{
    public required string Name { get; set; }
    public IList<UserTeam> Teams { get; set; } = [];
    public IList<Game> Schedule { get; set; } = [];
    public required int Season { get; set; }
    public int CurrentWeek { get; set; }
    public required LeagueSettings Settings { get; set; }
}