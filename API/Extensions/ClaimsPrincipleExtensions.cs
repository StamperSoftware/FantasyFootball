using System.Security.Authentication;
using System.Security.Claims;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ClaimsPrincipleExtensions
{
    public static async Task<AppUser> GetUserByEmail(this UserManager<AppUser> userManager, ClaimsPrincipal user)
    {
        var response = await userManager.Users.FirstOrDefaultAsync(u => u.Email == user.GetEmail()) ?? throw new AuthenticationException("Could not find user");
        return response;
    }

    private static string GetEmail(this ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email) ?? throw new AuthenticationException("Could not find user");
        return email;
    }
}