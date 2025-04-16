using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities;


public class AppUser : IdentityUser
{
    public AppUser(){}
    
    public AppUser(string email, string userName)
    {
        Email = email;
        UserName = userName;
    }
}