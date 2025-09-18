using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public sealed class AppUser : IdentityUser
{
    public static AppUser CreateAppUser(string email, string userName)
    {
        return new AppUser
        {
            Email = email,
            UserName = userName,
        };
    }
}