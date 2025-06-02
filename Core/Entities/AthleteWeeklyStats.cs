using System.Diagnostics.CodeAnalysis;

namespace Core.Entities;

public class AthleteWeeklyStats : BaseEntity
{
    public int Receptions { get; set; }
    public int ReceivingYards { get; set; }
    public int ReceivingTouchdowns { get; set; }
    public int RushingYards { get; set; }
    public int RushingTouchdowns { get; set; }
    public int PassingYards { get; set; }
    public int PassingTouchdowns { get; set; }

    public int Week { get; set; }
    public int Season { get; set; }

    public int AthleteId { get; set; }
    public required Athlete Athlete { get; set; }

    public AthleteWeeklyStats() { }

    [SetsRequiredMembers]
    public AthleteWeeklyStats(Athlete athlete, int week, int season, int receptions, int receivingYards, int receivingTouchdowns, int passingYards,
        int passingTouchdowns, int rushingYards, int rushingTouchdowns)
    {
        AthleteId = athlete.Id;
        Athlete = athlete;
        Week = week;
        Season = season;
        
        Receptions = receptions;
        ReceivingTouchdowns = receivingTouchdowns;
        ReceivingYards = receivingYards;
        PassingTouchdowns = passingTouchdowns;
        PassingYards = passingYards;
        RushingTouchdowns = rushingTouchdowns;
        RushingYards = rushingYards;
    }

}