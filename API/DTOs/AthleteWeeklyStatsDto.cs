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

    public AthleteWeeklyStatsDto(AthleteWeeklyStats aws)
    {
        Receptions = aws.Receptions;
        PassingTouchdowns = aws.PassingTouchdowns;
        PassingYards = aws.PassingYards;
        RushingTouchdowns = aws.RushingTouchdowns;
        RushingYards = aws.RushingYards;
        ReceivingTouchdowns = aws.ReceivingTouchdowns;
        ReceivingYards = aws.ReceivingYards;
        Score= Receptions + ((ReceivingTouchdowns + RushingTouchdowns + PassingTouchdowns) * 6) + ((ReceivingYards + RushingYards + PassingYards) /10);
        Week = aws.Week;
        Season = aws.Season;
        AthleteId = aws.AthleteId;
    }
}