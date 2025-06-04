namespace Core.Entities;

public class AthleteSeasonStats:BaseEntity
{
    public int AthleteId { get; set; }
    public int Season { get; set; }
    public int Receptions { get; set; }
    public int ReceivingYards { get; set; }
    public int ReceivingTouchdowns { get; set; }
    public int RushingYards { get; set; }
    public int RushingTouchdowns { get; set; }
    public int PassingYards { get; set; }
    public int PassingTouchdowns { get; set; }

    public AthleteSeasonStats() {}

    public AthleteSeasonStats(IList<AthleteWeeklyStats> weeklyStats)
    {
        foreach (var stat in weeklyStats)
        {
            Receptions += stat.Receptions;
            ReceivingTouchdowns += stat.ReceivingTouchdowns;
            ReceivingYards += stat.ReceivingYards;
            PassingTouchdowns += stat.PassingTouchdowns;
            PassingYards += stat.PassingYards;
            RushingTouchdowns += stat.RushingTouchdowns;
            RushingYards += stat.RushingYards;
        }
    }
}