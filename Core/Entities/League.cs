namespace Core.Entities;

public class League : BaseEntity
{
    public required string Name { get; set; }
    public IList<UserTeam> Teams { get; set; } = [];
}