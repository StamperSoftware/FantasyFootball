namespace Core.Entities;

public class AthleteSeasonStats(int athleteId, int season):BaseEntity
{
    public int AthleteId { get; set; } = athleteId;
    public int Season { get; set; } = season;
    public int Receptions { get; set; }
    public int ReceivingYards { get; set; }
    public int ReceivingTouchdowns { get; set; }
    public int RushingYards { get; set; }
    public int RushingTouchdowns { get; set; }
    public int PassingYards { get; set; }
    public int PassingTouchdowns { get; set; }
}