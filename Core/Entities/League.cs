using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class League : BaseEntity
{
    public required string Name { get; set; }
    public IList<UserTeam> Teams { get; set; } = [];
    public required string AdminId { get; set; }
    [NotMapped] public IList<Game> Schedule { get; set; } = [];
    public required int Season { get; set; }
    public int CurrentWeek { get; set; }
    [NotMapped] public LeagueSettings Settings { get; set; } = null!;

    public static League CreateLeague(string name, int season, string adminId)
    {
        return new League
        {
            Name = name,
            Season = season,
            AdminId = adminId
        };
    }
}