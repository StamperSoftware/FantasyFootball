using Core.Entities;

namespace API.DTOs;

public class TeamDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    
    public TeamDto(){}

    public TeamDto(Team team)
    {
        Id = team.Id;
        Name = team.Name;
        Location = team.Location;
    }
}