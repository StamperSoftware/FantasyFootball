using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Player : BaseEntity
{
    
    [Required] public required string FirstName { get; set; }
    [Required] public required string LastName { get; set; }
    public required AppUser User { get; set; }
    public required string UserId { get; set; }

    public static Player CreateNewPlayer(string firstName, string lastName, AppUser user)
    {
        return new Player
        {
            User = user,
            UserId = user.Id,
            FirstName = firstName,
            LastName = lastName,
        };
    }
}