using Core.Entities;

namespace API.DTOs;

public class PlayerDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public AppUserDto AppUser { get; set; }

    public PlayerDto(Player player)
    {
        Id = player.Id;
        FirstName = player.FirstName;
        LastName = player.LastName;
        AppUser = new AppUserDto(player.User);
    }
    
}