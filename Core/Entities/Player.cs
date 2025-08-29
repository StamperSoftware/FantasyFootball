using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Core.Entities;

public class Player : BaseEntity
{
    
    [Required] public required string FirstName { get; set; }
    [Required] public required string LastName { get; set; }
    public AppUser User { get; set; }
    public required string UserId { get; set; }
    
    public Player() {}
    
    [SetsRequiredMembers]
    public Player(string firstName, string lastName, AppUser user)
    {
        User = user;
        UserId = user.Id;
        FirstName = firstName;
        LastName = lastName;
    }
    
}