using Core.Entities;
using Core.Interfaces;

namespace API.Helpers;

public class DraftHelper
{
    public DraftHelper(IList<UserTeam> teams, IList<Athlete> athletes, int rounds)
    {
        Teams = teams;
        AvailableAthletes = athletes;
        CurrentPick = Teams[0];
        Index = 0;
        Status = DraftStatus.InProgress;
        Rounds = rounds;
        CreateSnakeDraftOrder();
    }

    public int Index { get; set; }
    public IList<UserTeam> Teams { get; set; }
    public IList<Athlete> AvailableAthletes { get; set; }
    public UserTeam CurrentPick { get; set; }
    public int Rounds { get; set; }
    public IList<DraftSlot> DraftOrder { get; set; } = [];
    public DraftStatus Status { get; set; }
    
    public void AdvancePick()
    {
        if (++Index == DraftOrder.Count)
        {
            Status = DraftStatus.Completed;
            return;
        }
        
        var nextTeamId = DraftOrder.First(s => s.Position == Index).TeamId;
        CurrentPick = Teams.First(t => t.Id == nextTeamId);
    }

    public Dictionary<int, IList<int>> GetDraftResults()
    {
        Dictionary<int, IList<int>> teamAthleteDictionary = [];
        foreach (var slot in DraftOrder)
        {
            if (slot.Athlete is null) continue;
            if (teamAthleteDictionary.TryGetValue(slot.TeamId, out var athleteIds))
            {
                athleteIds.Add(slot.Athlete.Id);
            }
            else
            {
                teamAthleteDictionary.Add(slot.TeamId, [slot.Athlete.Id]);
            }
        }

        return teamAthleteDictionary;
    }
    
    public void DraftAthlete(int athleteId, int teamId)
    {
        var slot = DraftOrder.First(s => s.Position == Index);
        var athlete = AvailableAthletes.First(a => a.Id == athleteId);
        slot.Athlete = athlete;
        AvailableAthletes.Remove(athlete);
        Teams.First(t => t.Id == teamId).Roster?.Bench.Add(athlete);
    }

    private void CreateSnakeDraftOrder()
    {
        var isIncreasing = true;
        DraftOrder = [];
        for (var i = 0; i < Teams.Count * Rounds; i++)
        {
            var teamIndex = i % Teams.Count;
            if (i > 0 && teamIndex == 0) isIncreasing = !isIncreasing;
            var currentTeam = isIncreasing ? Teams[teamIndex] : Teams[^(teamIndex + 1)];
            var displayOrder = isIncreasing ? i : (int)((Teams.Count + (Teams.Count * Math.Floor((double)i/Teams.Count))) - teamIndex)-1;
            DraftOrder.Add(new DraftSlot(i,displayOrder, currentTeam.Id));
        }
    }
}

public class DraftSlot(int position, int displayOrder, int teamId)
{
    public int Position { get; set; } = position;
    public int DisplayOrder { get; set; } = displayOrder;
    public int TeamId { get; set; } = teamId;
    public Athlete? Athlete { get; set; }
}

public enum DraftStatus {
    PreDraft, InProgress, Completed
} 
        
