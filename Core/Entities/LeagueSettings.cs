namespace Core.Entities;

public class LeagueSettings(int leagueId):BaseEntity
{
    public int LeagueId { get; set; } = leagueId;
    public int NumberOfGames { get; set; } = 12;
    public int NumberOfTeams { get; set; } = 10;
    public int RosterLimit { get; set; } = 20;
    public int StartingQuarterBackLimit { get; set; } = 1;
    public int StartingRunningBackLimit { get; set; } = 2;
    public int StartingWideReceiverLimit { get; set; } = 3;
    public int StartingTightEndLimit { get; set; } = 1;
    public int FlexLimit { get; set; } = 0;
    
    public int ReceptionScore { get; set; } = 1;
    public double ReceivingYardsScore { get; set; } = .1;
    public int ReceivingTouchdownsScore { get; set; } = 6;
    public double RushingYardsScore { get; set; } = .1;
    public int RushingTouchdownsScore { get; set; } = 6;
    public double PassingYardsScore { get; set; } = .1;
    public int PassingTouchdownsScore { get; set; } = 6;
}