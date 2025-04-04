namespace Core.Entities;

public class League : BaseEntity
{
    public required string Name { get; set; }
    public List<AppUser> Players { get; set; } = [];
}