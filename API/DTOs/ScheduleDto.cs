using Core.Entities;

namespace API.DTOs;

public class ScheduleDto
{
    public IList<GameDto> Games { get; set; } = [];
    public int LeagueId { get; set; }

    public ScheduleDto(Schedule schedule)
    {
        Games = schedule.Games.Select(game => new GameDto(game)).ToList();
        LeagueId = schedule.LeagueId;
    }
}