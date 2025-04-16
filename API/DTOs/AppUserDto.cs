using Core.Entities;

namespace API.DTOs;

public class AppUserDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    
    public AppUserDto(AppUser user)
    {
        Id = user.Id;
        Email = user.Email ?? "";
        UserName = user.UserName ?? "";
    }

    public AppUserDto(RegisterDto dto)
    {
        Email = dto.Email;
        UserName = dto.UserName;
    }
}