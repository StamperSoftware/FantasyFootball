using Core.Entities;

namespace API.DTOs;

public class AthleteWeeklyStatsDto
{
    public int Receptions { get; set; } = 0;
    public int ReceivingYards { get; set; } = 0;
    public int ReceivingTouchdowns { get; set; } = 0;
    public int RushingYards { get; set; } = 0;
    public int RushingTouchdowns { get; set; } = 0;
    public int PassingYards { get; set; } = 0;
    public int PassingTouchdowns { get; set; } = 0;
    public int Week { get; set; }
    public int Season { get; set; }
    public int AthleteId { get; set; }

    public int Score { get; set; } 

}