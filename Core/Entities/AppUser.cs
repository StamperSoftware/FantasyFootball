using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public sealed class AppUser : IdentityUser
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    
    public static AppUser CreateAppUser(string email, string userName, string firstName, string lastName)
    {
        return new AppUser
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            UserName = userName,
        };
    }
}