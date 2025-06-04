namespace API.DTOs;

public class UpdateAthleteStatsRequest
{
    public int Week { get; set; }
    public int Season { get; set; }
    public int Receptions { get; set; } = 0;
    public int ReceivingYards { get; set; } = 0;
    public int ReceivingTouchdowns { get; set; } = 0;
    public int RushingYards { get; set; } = 0;
    public int RushingTouchdowns { get; set; } = 0;
    public int PassingYards { get; set; } = 0;
    public int PassingTouchdowns { get; set; } = 0;
}