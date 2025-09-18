using Core.Entities;
using Core.Interfaces;

namespace API.Helpers;

public class DraftHelper
{
    public static DraftHelper Create(IList<UserTeam> teams, IList<Athlete> athletes, int rounds)
    {
        return new DraftHelper
        {
            Teams = teams,
            AvailableAthletes = athletes,
            CurrentPick = teams[0],
            Index = 0,
            Status = DraftStatus.PreDraft,
            DraftOrder = CreateSnakeDraftOrder(teams, rounds),
        };
    }
    
    public int Index { get; set; }
    public IList<UserTeam> Teams { get; set; }
    public IList<Athlete> AvailableAthletes { get; set; }
    public UserTeam CurrentPick { get; set; }
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

    private static IList<DraftSlot> CreateSnakeDraftOrder(IList<UserTeam> teams, int rounds)
    {
        var isIncreasing = true;
        IList<DraftSlot> order = [];
        for (var i = 0; i < teams.Count * rounds; i++)
        {
            var teamIndex = i % teams.Count;
            if (i > 0 && teamIndex == 0) isIncreasing = !isIncreasing;
            var currentTeam = isIncreasing ? teams[teamIndex] : teams[^(teamIndex + 1)];
            var displayOrder = isIncreasing ? i : (int)((teams.Count + (teams.Count * Math.Floor((double)i/teams.Count))) - teamIndex)-1;
            order.Add(new DraftSlot(i,displayOrder, currentTeam.Id));
        }

        return order;
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
        
