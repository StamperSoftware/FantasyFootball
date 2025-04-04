using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Player : BaseEntity
{
    
    [Required] public string FirstName { get; set; } = "";
    [Required] public string LastName { get; set; } = "";
    public AppUser User { get; set; }
    public required string UserId { get; set; }
}