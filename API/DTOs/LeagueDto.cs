using Core.Entities;

namespace API.DTOs;

public class LeagueDto
{
    public int Id { get; set; }
    public IList<UserTeamDto> Teams { get; set; }
    public string Name { get; set; }
    public ScheduleDto? Schedule { get; set; }
    public int Season { get; set; }

    public LeagueDto(League league)
    {
        Id = league.Id;
        Teams = league.Teams.Select(team => new UserTeamDto(team)).ToList();
        Name = league.Name;
        Season = league.Season;
        
        if (league.Schedule != null)
        {
            Schedule = new ScheduleDto(league.Schedule);
        }
    }
}