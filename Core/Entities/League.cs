namespace Core.Entities;

public class League : BaseEntity
{
    public required string Name { get; set; }
    public IList<UserTeam> Teams { get; set; } = [];
    public int NumberOfGames { get; set; }
    public Schedule? Schedule { get; set; }
    public required int Season { get; set; }
    public int CurrentWeek { get; set; }
}