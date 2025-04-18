using Core.Entities;

namespace API.DTOs;

public class AthleteDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public TeamDto Team { get; set; }
    public PositionDto Position { get; set; }
    
    public AthleteDto(){}

    public AthleteDto(Athlete athlete)
    {
        Id = athlete.Id;
        FirstName = athlete.FirstName;
        LastName = athlete.LastName;
        Team = new TeamDto(athlete.Team);
        Position = (PositionDto) athlete.Position;
    }
    
}
