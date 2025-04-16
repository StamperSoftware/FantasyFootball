using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.Entities;

public class Player : BaseEntity
{
    
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    public required AppUser User { get; set; }
    public required string UserId { get; set; }
    public List<UserTeam> Teams { get; }
    
    [SetsRequiredMembers]
    public Player(string firstName, string lastName, AppUser user)
    {
        User = user;
        UserId = user.Id;
        FirstName = firstName;
        LastName = lastName;
        Teams = [];
    }
    
}