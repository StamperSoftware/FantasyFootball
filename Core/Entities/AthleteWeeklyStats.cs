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

    public static AthleteWeeklyStats Create(int week, int season, int athleteId, int receptions, int receivingYards, int receivingTouchdowns, int passingYards, int passingTouchdowns, int rushingYards, int rushingTouchdowns)
    {
        return new AthleteWeeklyStats
        {
            Week = week,
            Season = season,
            AthleteId = athleteId,
            Receptions = receptions,
            ReceivingYards = receivingYards,
            ReceivingTouchdowns = receivingTouchdowns,
            PassingTouchdowns = passingTouchdowns,
            PassingYards = passingYards,
            RushingTouchdowns = rushingTouchdowns,
            RushingYards = rushingYards
        };
    }
}  