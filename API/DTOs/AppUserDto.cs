using System.Diagnostics.CodeAnalysis;
using Core.Entities;

namespace API.DTOs;

public class AppUserDto
{
    public required string Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    
    [SetsRequiredMembers]
    public AppUserDto(AppUser user)
    {
        Id = user.Id;
        Email = user.Email ?? "";
        UserName = user.UserName ?? "";
    }
}