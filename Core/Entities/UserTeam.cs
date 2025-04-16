namespace Core.Entities;

public class UserTeam : BaseEntity
{
    public int LeagueId { get; set; }
    public int PlayerId { get; set; }
    public required Player Player { get; set; } = null!;
    public string? Name { get; set; }

        
}