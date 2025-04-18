namespace Core.Entities;

public class Team : BaseEntity
{
    
    public string Name { get; set; } = "";
    public string Location { get; set; } = "";
    public IList<Athlete> Athletes { get; set; } = [];
}