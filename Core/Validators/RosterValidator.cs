using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Core.Validators;

public static class RosterValidator
{
    
    public static ValidationResult Validate(this Roster roster, LeagueSettings leagueSettings)
    {
        IList<string> errors = [];
        if (roster.Starters.Count + roster.Bench.Count > leagueSettings.RosterLimit) errors.Add("Roster Limit Reached");
        if (roster.Starters.Count(a => a.Position == Position.QuarterBack) > leagueSettings.StartingQuarterBackLimit) errors.Add("Quarter Back Starting Limit Reached");
        if (roster.Starters.Count(a => a.Position == Position.WideReceiver) > leagueSettings.StartingWideReceiverLimit) errors.Add("Wide Receiver Starting Limit Reached");
        if (roster.Starters.Count(a => a.Position == Position.RunningBack) > leagueSettings.StartingRunningBackLimit) errors.Add("Running Back Starting Limit Reached");
        if (roster.Starters.Count(a => a.Position == Position.TightEnd) > leagueSettings.StartingTightEndLimit) errors.Add("Tight End Starting Limit Reached");
        
        return errors.Any() ? new ValidationResult(string.Join(", ", errors)) : ValidationResult.Success!;
    }
}