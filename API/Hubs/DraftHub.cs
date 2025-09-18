using API.Helpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.SignalR;
namespace API.Hubs;

public class DraftHub:Hub
{
    private static Dictionary<int, DraftHelper> ConnectedLeagues { get; set; } = [];
    private static IList<Athlete> Athletes { get; set; } = [];

    private ILeagueService LeagueService { get; set; }
    private IAthleteService AthleteService { get; set; }
    private readonly Random _random = new();
    
    public DraftHub(ILeagueService leagueService, IAthleteService athleteService)
    {
        AthleteService = athleteService;
        LeagueService = leagueService;
        Athletes = AthleteService.GetAthletes().Result;
        Athletes = Athletes.OrderBy(a => a.Position).ThenBy(a => a.LastName).ToList();
    }
    
    public async Task<DraftHelper> JoinGroup(int leagueId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, leagueId.ToString());
        
        if (ConnectedLeagues.TryGetValue(leagueId, out var draftHelper))
        {
            var userTeam = draftHelper.Teams.FirstOrDefault(ut => ut.Player.User.Id == Context.UserIdentifier) ?? throw new Exception("Could not get team");
            userTeam.SetOnline();
            await Clients.GroupExcept(leagueId.ToString(), [Context.ConnectionId]).SendAsync("JoinedGroup", draftHelper);
        }
        else
        {
            var league = await LeagueService.GetLeagueWithFullDetailsAsync(leagueId) ?? throw new Exception("Could not get league");
            var userTeam = league.Teams.FirstOrDefault(ut => ut.Player.User.Id == Context.UserIdentifier) ?? throw new Exception("Could not get team");
            var userTeams = league.Teams.OrderBy(t => league.Settings.DraftOrder.IndexOf(t.Id)).ToList();
            userTeam.SetOnline();
            draftHelper = DraftHelper.Create(userTeams, Athletes.ToList(), league.Settings.RosterLimit);
            ConnectedLeagues[leagueId] = draftHelper;
        }
        
        return draftHelper;
    }
    
    public async Task<DraftHelper?> LeaveGroup(int leagueId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, leagueId.ToString());
        var userTeam = ConnectedLeagues[leagueId].Teams.FirstOrDefault(ut => ut.Player.User.Id == Context.UserIdentifier) ?? throw new Exception("Could not get team");
        userTeam.SetOffline();
        if (!ConnectedLeagues[leagueId].Teams.Any(t => t.IsOnline))
        {
            return null;
        }
        
        await Clients.GroupExcept(leagueId.ToString(), [Context.ConnectionId]).SendAsync("LeftGroup", ConnectedLeagues[leagueId]);
        return ConnectedLeagues[leagueId];
    }
    
    public async Task<DraftHelper> DraftPlayer(int leagueId, int teamId, int athleteId)
    {
        var draftHelper = ConnectedLeagues[leagueId];
        draftHelper.DraftAthlete(athleteId, teamId);
        draftHelper.AdvancePick();
        if (draftHelper.Status == DraftStatus.Completed)
        {
            await LeagueService.SubmitDraft(leagueId, draftHelper.GetDraftResults());
        }
        await Clients.GroupExcept(leagueId.ToString(), [Context.ConnectionId]).SendAsync("DraftedPlayer", draftHelper);
        return draftHelper;
    }

    public async Task<DraftHelper> SimulateDraft(int leagueId)
    {
        var draftHelper = ConnectedLeagues[leagueId];
        do
        {
            draftHelper = await DraftPlayer(leagueId, draftHelper.CurrentPick.Id, draftHelper.AvailableAthletes.OrderBy(a => _random.Next()).First().Id);
        } while (draftHelper.Status == DraftStatus.InProgress);

        return draftHelper;
    }

}