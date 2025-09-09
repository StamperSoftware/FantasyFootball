namespace Core.Entities;

public class UploadAthleteStatistics
{
    public int AthleteId { get; set; }
    public int Receptions { get; set; }
    public int ReceivingYards { get; set; }
    public int ReceivingTouchdowns { get; set; }
    public int PassingYards { get; set; }
    public int PassingTouchdowns { get; set; }
    public int RushingYards { get; set; }
    public int RushingTouchdowns { get; set; }
}