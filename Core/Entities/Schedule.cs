namespace Core.Entities;

public class Schedule(int leagueId):BaseEntity
{

    public IList<Game> Games { get; set; } = [];
    public int LeagueId { get; set; } = leagueId;

    public void PrintSchedule()
    {
        Console.WriteLine($"Schedule for league {LeagueId}");
        Console.WriteLine($"-----------------------");
        Console.WriteLine();
        Console.WriteLine();
        
        foreach (var kvp in Games.GroupBy(g => g.Week))
        {
            Console.WriteLine($"Week: {kvp.Key}");
            foreach (var game in kvp)
            {
                Console.WriteLine(game);
            }
            Console.WriteLine();
        }
    }
}

