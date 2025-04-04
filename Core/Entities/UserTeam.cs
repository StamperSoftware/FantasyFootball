namespace Core.Entities;

public class UserTeam : BaseEntity
{
    public int LeagueId { get; set; }
    public int PlayerId { get; set; }
    public Player Player { get; set; }
}