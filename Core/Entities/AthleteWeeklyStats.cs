using System.Diagnostics.CodeAnalysis;

namespace Core.Entities;

public class AthleteWeeklyStats(int week, int season, int athleteId) : BaseEntity
{
    public int Receptions { get; set; }
    public int ReceivingYards { get; set; }
    public int ReceivingTouchdowns { get; set; }
    public int RushingYards { get; set; }
    public int RushingTouchdowns { get; set; }
    public int PassingYards { get; set; }
    public int PassingTouchdowns { get; set; }
    public int Week { get; set; } = week;
    public int Season { get; set; } = season;
    public int AthleteId { get; set; } = athleteId;
}