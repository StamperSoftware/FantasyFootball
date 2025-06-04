using System.Diagnostics.CodeAnalysis;
using Core.Entities;

namespace API.DTOs;

public class TeamDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Location { get; set; }

    [SetsRequiredMembers]
    public TeamDto(Team team)
    {
        Id = team.Id;
        Name = team.Name;
        Location = team.Location;
    }
}