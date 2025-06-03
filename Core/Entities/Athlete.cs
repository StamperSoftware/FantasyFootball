namespace Core.Entities;

public class Athlete : BaseEntity
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public int TeamId { get; set; }
    public required Team Team { get; set; }
    public Position Position { get; set; }
}

public enum Position
{
    QuarterBack=1, RunningBack, WideReceiver, TightEnd, Defense  
}