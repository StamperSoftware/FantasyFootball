namespace API.DTOs;

public class AthleteDto
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public int TeamId { get; set; }
    public TeamDto? Team { get; set; }
    public PositionDto Position { get; set; }
}
