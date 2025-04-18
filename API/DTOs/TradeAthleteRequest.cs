namespace API.DTOs;

public class TradeAthleteRequest
{
    public int TeamOneId { get; set; }
    public int TeamTwoId { get; set; }
    public IList<int> TeamOneAthleteIds { get; set; } = [];
    public IList<int> TeamTwoAthleteIds { get; set; } = [];
}