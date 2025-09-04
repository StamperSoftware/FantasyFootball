using Core.Entities;

namespace API.DTOs;

public class PlayerDto
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string UserId { get; set; }
    public required string UserName { get; set; }
}