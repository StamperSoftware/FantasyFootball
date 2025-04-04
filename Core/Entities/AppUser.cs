using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities;


public class AppUser : IdentityUser
{
    public List<League> Leagues { get; set; } = [];

    [Required] public string FirstName { get; set; } = "";
    [Required] public string LastName { get; set; } = "";
}