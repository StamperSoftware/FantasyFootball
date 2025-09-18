using API.DTOs;
using Core.Entities;

namespace API.Extensions;

public static class DtoConverter
{
    public static GameDto Convert(this Game game)
    {
        return new GameDto
        {
            Id = game.Id,
            HomeId = game.HomeId,
            Home = game.Home.Convert(),
            AwayId = game.HomeId,
            Away = game.Away.Convert(),
            Week = game.Week,
            Season = game.Season,
            WeeklyStats = game.WeeklyStats.Select(aws => aws.Convert()).ToList(),
            HomeScore = game.HomeScore,
            AwayScore = game.AwayScore,
            IsFinalized = game.IsFinalized,
        };
    }

    public static UserTeamDto Convert(this UserTeam team)
    {
        return new UserTeamDto
        {
            Id = team.Id,
            LeagueId = team.LeagueId,
            Player = team.Player.Convert(),
            Name = team.Name,
            RosterId = team.RosterId,
            Roster = team.Roster,
            Wins = team.Wins,
            Losses = team.Losses,
            Ties = team.Ties,
        };
    }

    public static LeagueDto Convert(this League league)
    {
        return new LeagueDto
        {
            Id = league.Id,
            Teams = league.Teams.Select(t => t.Convert()).ToList(),
            Name = league.Name,
            Season = league.Season,
            Settings = league.Settings,
            Schedule = league.Schedule.Select(g => g.Convert()).ToList(),
            Admin = league.Admin.Convert(),
        };
    }

    public static TeamDto Convert(this Team team)
    {
        return new TeamDto
        {
            Id = team.Id,
            Name = team.Name,
            Location = team.Location,
        };
    }

    public static PlayerDto Convert(this Player player)
    {
        return new PlayerDto
        {
            Id = player.Id,
            FirstName = player.FirstName,
            LastName = player.LastName,
            UserId = player.UserId,
            UserName = player.User.UserName!,
        };
    }

    public static AppUserDto Convert(this AppUser user)
    {
        return new AppUserDto
        {
            Id = user.Id,
            Email = user.Email ?? "",
            UserName = user.UserName ?? "",
        };
    }

    public static AthleteWeeklyStatsDto Convert(this AthleteWeeklyStats aws)
    {
        return new AthleteWeeklyStatsDto
        {
            Receptions = aws.Receptions,
            PassingTouchdowns = aws.PassingTouchdowns,
            PassingYards = aws.PassingYards,
            RushingTouchdowns = aws.RushingTouchdowns,
            RushingYards = aws.RushingYards,
            ReceivingTouchdowns = aws.ReceivingTouchdowns,
            ReceivingYards = aws.ReceivingYards,
            Score = aws.Receptions + ((aws.ReceivingTouchdowns + aws.RushingTouchdowns + aws.PassingTouchdowns) * 6) + ((aws.ReceivingYards +
            aws.RushingYards + aws.PassingYards) /10),
            Week = aws.Week,
            Season = aws.Season,
            AthleteId = aws.AthleteId,
        };
    }

    public static AthleteDto Convert(this Athlete athlete)
    {
        return new AthleteDto
        {
            Id = athlete.Id,
            FirstName = athlete.FirstName,
            LastName = athlete.LastName,
            TeamId = athlete.TeamId,
            Team = athlete.Team.Convert(),
            Position = (PositionDto)athlete.Position,
        };
    }

    public static TradeRequestTeamDto ConvertReceived(this TradeRequest request)
    {
        return new TradeRequestTeamDto
        {
            MyPlayers = request.ReceivingAthletes.Select(a => a.Convert()).ToList(),
            TheirPlayers = request.InitiatingAthletes.Select(a => a.Convert()).ToList(),
            Id = request.Id
        };
    }
    
    public static TradeRequestTeamDto ConvertInitiated(this TradeRequest request)
    {
        return new TradeRequestTeamDto
        {
            MyPlayers = request.InitiatingAthletes.Select(a => a.Convert()).ToList(),
            TheirPlayers = request.ReceivingAthletes.Select(a => a.Convert()).ToList(),
            Id = request.Id
        };
    }
    
}