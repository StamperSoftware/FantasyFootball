using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Entities;

public class Athlete : BaseEntity
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public int TeamId { get; set; }
    public required Team Team { get; set; }
    public Position Position { get; set; }
    public IList<AthleteWeeklyStats> WeeklyStats { get; set; } = [];
    public AthleteSeasonStats SeasonStats { get; set; } = new();
}

public enum Position
{
    QuarterBack=1, RunningBack, WideReceiver, TightEnd, Defense  
}